using AutoHotkey.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.Core;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen.RawInput
{
	class MessageProcessor
	{
		private static readonly System.Drawing.Rectangle clipRect = 
			new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(1, 1));

		/// <summary>
		/// Only updated when split screen is deactivated
		/// </summary>
		public IntPtr LastKeyboardPressed { get; private set; } = new IntPtr(0);
		
		//leftMiddleRight: left=1, middle=2, right=3, xbutton1=4, xbutton2=5
		readonly Dictionary<ButtonFlags, (MouseInputNotifications msg, uint wParam, ushort leftMiddleRight, bool isButtonDown, int VKey)> ButtonFlagToMouseInputNotifications = new Dictionary<ButtonFlags, (MouseInputNotifications, uint, ushort, bool, int)>()
		{
			{ ButtonFlags.RI_MOUSE_LEFT_BUTTON_DOWN,	(MouseInputNotifications.WM_LBUTTONDOWN ,	0x0001,		1, true,    0x01) },
			{ ButtonFlags.RI_MOUSE_LEFT_BUTTON_UP,		(MouseInputNotifications.WM_LBUTTONUP,		0,			1, false,   0x01) },

			{ ButtonFlags.RI_MOUSE_RIGHT_BUTTON_DOWN,	(MouseInputNotifications.WM_RBUTTONDOWN,	0x0002,		2, true,    0x02) },
			{ ButtonFlags.RI_MOUSE_RIGHT_BUTTON_UP,		(MouseInputNotifications.WM_RBUTTONUP,		0,			2, false,   0x02) },

			{ ButtonFlags.RI_MOUSE_MIDDLE_BUTTON_DOWN,	(MouseInputNotifications.WM_MBUTTONDOWN,	0x0010,		3, true,    0x04) },
			{ ButtonFlags.RI_MOUSE_MIDDLE_BUTTON_UP,	(MouseInputNotifications.WM_MBUTTONUP,		0,			3, false,   0x04) },

			{ ButtonFlags.RI_MOUSE_BUTTON_4_DOWN,		(MouseInputNotifications.WM_XBUTTONDOWN,	0x0120,		4, true,    0x05) },// (0x0001 << 8) | 0x0020 = 0x0120
			{ ButtonFlags.RI_MOUSE_BUTTON_4_UP,			(MouseInputNotifications.WM_XBUTTONUP,		0,			4, false,   0x05) },

			{ ButtonFlags.RI_MOUSE_BUTTON_5_DOWN,		(MouseInputNotifications.WM_XBUTTONDOWN,    0x0240,		5, true,    0x06) },//(0x0002 << 8) | 0x0040 = 0x0240
			{ ButtonFlags.RI_MOUSE_BUTTON_5_UP,			(MouseInputNotifications.WM_XBUTTONUP,		0,			5, false,   0x06) }
		};

		#region End key
		private ushort endVKey = 0x23;//End. 0x23 = 35
		private bool WaitingToSetEndKey = false;


		public void WaitToSetEndKey()
		{
			WaitingToSetEndKey = true;
			Program.Form.SetEndButtonText("Press a key...");
		}

		public void StopWaitingToSetEndKey()
		{
			WaitingToSetEndKey = false;
			Program.Form.SetEndButtonText($"Stop button = {System.Windows.Input.KeyInterop.KeyFromVirtualKey(endVKey)}");
			Options.CurrentOptions.EndVKey = endVKey;
		}
		#endregion


		AutoResetEvent autoEvent = new AutoResetEvent(false);
		Queue<(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)> msgs = new Queue<(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)>();
		Thread thread;

		public MessageProcessor()
		{
			endVKey = Options.CurrentOptions.EndVKey;

			thread = new Thread(MessageLoop);
			thread.Start();
		}

		public void WndProc(ref Message msg)
		{
			if (msg.Msg == WinApi.WM_INPUT)
			{
				IntPtr hRawInput = msg.LParam;

				Process(hRawInput);
			}
		}

		void MessageLoop()
		{
			while (true)
			{
				if (msgs.Count == 0)
					autoEvent.WaitOne();

				var (hWnd, Msg, wParam, lParam) = msgs.Dequeue();
				SendInput.WinApi.PostMessageA(hWnd, Msg, wParam, lParam);
			}
		}

		void PostMessageA(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
		{
			SendInput.WinApi.PostMessageA(hWnd, Msg, wParam, lParam);
			return;
			//TODO: Re-enable ? (Broke starbound)

			msgs.Enqueue((hWnd, Msg, wParam, lParam));

			if (msgs.Count == 0)
				autoEvent.Set();
		}

		private void Process(IntPtr hRawInput)
		{
			uint pbDataSize = 0;
			/*Return Value (of GetRawInputData)
			Type: UINT
			If pData is NULL and the function is successful, the return value is 0.If pData is not NULL and the function is successful, the return value is the number of bytes copied into pData.
			If there is an error, the return value is (UINT) - 1.*/
			WinApi.GetRawInputData(hRawInput, DataCommand.RID_INPUT, IntPtr.Zero, ref pbDataSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

			if (pbDataSize == WinApi.GetRawInputData(hRawInput, DataCommand.RID_INPUT, out RAWINPUT rawBuffer, ref pbDataSize, Marshal.SizeOf(typeof(RAWINPUTHEADER))))
			{
				switch ((HeaderDwType)rawBuffer.header.dwType)
				{
					case HeaderDwType.RIM_TYPEKEYBOARD:
						{
							uint keyboardMessage = rawBuffer.data.keyboard.Message;
							bool keyUpOrDown = keyboardMessage == (uint)KeyboardMessages.WM_KEYDOWN || keyboardMessage == (uint)KeyboardMessages.WM_KEYUP;
							
							//Console.WriteLine($"KEYBOARD. key={rawBuffer.data.keyboard.VKey:x}, message = {rawBuffer.data.keyboard.Message:x}, device pointer = {rawBuffer.header.hDevice}");

							if (!Program.SplitScreenManager.IsRunningInSplitScreen)
							{
								if (keyUpOrDown)
								{
									if (WaitingToSetEndKey)
									{
										endVKey = rawBuffer.data.keyboard.VKey;
										StopWaitingToSetEndKey();
									}

									LastKeyboardPressed = rawBuffer.header.hDevice;
									break;
								}
							}
							else
							{ 
								if (keyUpOrDown && rawBuffer.data.keyboard.VKey == endVKey)//End key
								{
									Console.WriteLine("End key pressed");
									Intercept.InterceptEnabled = false;
									Program.SplitScreenManager.DeactivateSplitScreen();
									InputDisabler.Unlock();//Just in case
								}

								if (keyUpOrDown)
								{
									/**string cmd = string.Format("ControlSend, , {{vk{0:x} {1}}}, ahk_id {2:x}", VKey, keyboardMessage == (uint)KeyboardMessages.WM_KEYDOWN ? "down" : "up", hWnd);
									//Console.WriteLine(cmd);

									if (0x73 != rawBuffer.data.keyboard.VKey)//f4
									{
										/**
										AutoHotkeyEngine ahk;
										if (ahks.TryGetValue(rawBuffer.header.hDevice, out var ahk0))
											ahk = ahk0;
										else
										{
											ahk = AutoHotkeyEngine.Instance;
											ahks[rawBuffer.header.hDevice] = ahk;
										}*
										
										//AHKThread.AddCommand(cmd);
									}*/

									foreach (Window window in Program.SplitScreenManager.GetWindowsForDevice(rawBuffer.header.hDevice))
									{
										IntPtr hWnd = window.hWnd;

										//Works better than ahk (doesn't lock mouse when two keyboards are held, and works with Source games)
										if (Options.CurrentOptions.SendNormalKeyboardInput)
										{
											uint scanCode = rawBuffer.data.keyboard.MakeCode;
											ushort VKey = rawBuffer.data.keyboard.VKey;

											bool keyDown = keyboardMessage == (uint)KeyboardMessages.WM_KEYDOWN;

											//uint code = 0x000000000000001 | (scanCode << 16);//32-bit
											uint code = (scanCode << 16);//32-bit

											var keysDown = window.keysDown;

											if (keyDown)
											{
												//bit 30 : The previous key state. The value is 1 if the key is down before the message is sent, or it is zero if the key is up.
												if (keysDown.TryGetValue(VKey, out bool wasDown) && wasDown)
												{
													code |= 0x40000000;
												}
											}
											else
											{
												code |= 0xC0000000;//WM_KEYUP required the bit 31 and 30 to be 1
												code |= 0x000000000000001;
											}

											keysDown[VKey] = keyDown;

											//if (VKey == 0x41 || VKey == 0x44 || VKey == 0x53 || VKey == 0x57)//WASD
											//{
											//	window.GetRawInputData_HookServer?.SetVKey(VKey, keyDown);
											//	window.HooksCPPNamedPipe?.AddMessage(0x02, VKey, keyDown ? 1 : 0);
											//}
											
											byte shift = (byte)(VKey == 0x41 ? 0b0001 : (VKey == 0x44 ? 0b0010 : (VKey == 0x53 ? 0b0100 : (VKey == 0x57 ? 0b1000 : 0))));
											if (((window.WASD_State & shift) == 0 ? false : true) != keyDown)
											{
												if (keyDown)
													window.WASD_State |= shift;
												else
													window.WASD_State &= (byte)~shift;
												
												window.HooksCPPNamedPipe?.WriteMessage(0x02, VKey, keyDown ? 1 : 0);
												
											}

											//This also makes GetKeyboardState work, as windows uses the message queue for GetKeyboardState
											SendInput.WinApi.PostMessageA(hWnd, keyboardMessage, (IntPtr)VKey, (UIntPtr)code);
										}

										//Resend raw input to application. Works for some games only
										if (Options.CurrentOptions.SendRawKeyboardInput)
											PostMessageA(hWnd, (uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0001, (IntPtr)hRawInput);
									}
								}
							}

							break;
						}
					case HeaderDwType.RIM_TYPEMOUSE:
						{
							RAWMOUSE mouse = rawBuffer.data.mouse;
							IntPtr mouseHandle = rawBuffer.header.hDevice;

							if (!Program.SplitScreenManager.IsRunningInSplitScreen)
							{
								if ((mouse.usButtonFlags & (ushort)ButtonFlags.RI_MOUSE_LEFT_BUTTON_UP) > 0 && Program.Form.ButtonPressed)
								{
									Console.WriteLine($"Set mouse, pointer = {rawBuffer.header.hDevice}");
									Program.SplitScreenManager.SetMouseHandle(rawBuffer.header.hDevice);
								}
								break; 
							}

							var windows = Program.SplitScreenManager.GetWindowsForDevice(mouseHandle);
							for (int windowI = 0; windowI < windows.Length; windowI++)
							{
								Window window = windows[windowI];
								IntPtr hWnd = window.hWnd;

								//Resend raw input to application. Works for some games only
								if (Options.CurrentOptions.SendRawMouseInput)
								{
									/*IntPtr hptr = Marshal.AllocHGlobal((int)pbDataSize);
									WinApi.GetRawInputData(hRawInput, DataCommand.RID_INPUT, hptr, ref pbDataSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
									
									SendInput.WinApi.PostMessageA(window.borderlands2_DIEmWin_hWnd == IntPtr.Zero ? hWnd : window.borderlands2_DIEmWin_hWnd,
										(uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0000, hptr);*/
										

									SendInput.WinApi.PostMessageA(window.borderlands2_DIEmWin_hWnd == IntPtr.Zero ? hWnd : window.borderlands2_DIEmWin_hWnd, 
										(uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0000, hRawInput);
								}

								IntVector2 mouseVec = window.MousePosition;

								mouseVec.x = Math.Min(window.Width, Math.Max(mouseVec.x + mouse.lLastX, 0));
								mouseVec.y = Math.Min(window.Height, Math.Max(mouseVec.y + mouse.lLastY, 0));
								
								window.HooksCPPNamedPipe?.WriteMessage(0x01, mouseVec.x, mouseVec.y);
								
								//Console.WriteLine($"MOUSE. flags={mouse.usFlags}, X={mouseVec.x}, y={mouseVec.y}, buttonFlags={mouse.usButtonFlags} device pointer = {rawBuffer.header.hDevice}");

								long packedXY = (mouseVec.y * 0x10000) + mouseVec.x;

								//TODO: move away to reduce lag?
								//Cursor.Position = new System.Drawing.Point(0, 0);
								//Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(1, 1));

								if (Options.CurrentOptions.SendNormalMouseInput)
								{
									ushort mouseMoveState = 0x0000;
									var (l, m, r, x1, x2) = window.MouseState;
									if (l) mouseMoveState	|= (ushort)WM_MOUSEMOVE_wParam.MK_LBUTTON;
									if (m) mouseMoveState	|= (ushort)WM_MOUSEMOVE_wParam.MK_MBUTTON;
									if (r) mouseMoveState	|= (ushort)WM_MOUSEMOVE_wParam.MK_RBUTTON;
									if (x1) mouseMoveState	|= (ushort)WM_MOUSEMOVE_wParam.MK_XBUTTON1;
									if (x2) mouseMoveState	|= (ushort)WM_MOUSEMOVE_wParam.MK_XBUTTON2;
									mouseMoveState |= 0b10000000;//Signature for USS 
									PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEMOVE, (IntPtr)mouseMoveState, (IntPtr)packedXY);
									//PostMessageA(hWnd, (uint)MouseInputNotifications.WM_NCMOUSEMOVE, (IntPtr)0x0000, (IntPtr)packedXY);
								}

								//Mouse buttons.
								ushort f = mouse.usButtonFlags;
								if (f != 0)
								{
									foreach (var pair in ButtonFlagToMouseInputNotifications)
									{
										if ((f & (ushort)pair.Key) > 0)
										{
											var (msg, wParam, leftMiddleRight, isButtonDown, VKey) = pair.Value;
											//Console.WriteLine(pair.Key);
											SendInput.WinApi.PostMessageA(hWnd, (uint)msg, (IntPtr)wParam, (IntPtr)packedXY);

											//TODO: MAKE CONFIGURABLE FOR GetAsyncKeyState hook checkbox
											window.HooksCPPNamedPipe?.WriteMessage(0x02, VKey, isButtonDown ? 1 : 0);

											var state = window.MouseState;
											switch (leftMiddleRight)
											{
												case 1:
													state.l = isButtonDown;

													if (Options.CurrentOptions.RefreshWindowBoundsOnMouseClick)
														window.UpdateBounds();

													break;
												case 2:
													state.m = isButtonDown; break;
												case 3:
													state.r = isButtonDown; break;
												case 4:
													state.x1 = isButtonDown; break;
												case 5:
													state.x2 = isButtonDown; break;

											}
											window.MouseState = state;

											//Cursor.Position = new System.Drawing.Point(0, 0);
											//Cursor.Clip = clipRect;
										}
									}

									//TODO: re-enable mouse wheel
									if (Options.CurrentOptions.SendScrollwheel && (f & (ushort)ButtonFlags.RI_MOUSE_WHEEL) > 0)
									{
										ushort delta = mouse.usButtonData;
										PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEWHEEL, (IntPtr)((delta * 0x10000) + 0), (IntPtr)packedXY);
									}
								}
							}

							break;
						}
					default: break;//HID
				}
			}

		}
	}
}
