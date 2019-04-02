using AutoHotkey.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.Core;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen.RawInput
{
	class MessageProcessor//TODO make not static
	{
		/// <summary>
		/// Only updated when split screen is deactivated
		/// </summary>
		public static IntPtr LastKeyboardPressed { get; private set; } = new IntPtr(0);
		
		//TODO: xButton1/2
		//leftMiddleRight: left=1, middle=2, right=3
		readonly static Dictionary<ButtonFlags, (MouseInputNotifications msg, ushort wParam, ushort leftMiddleRight, bool isButtonDown)> ButtonFlagToMouseInputNotifications = new Dictionary<ButtonFlags, (MouseInputNotifications, ushort, ushort, bool)>()
		{
			{ ButtonFlags.RI_MOUSE_LEFT_BUTTON_DOWN , (MouseInputNotifications.WM_LBUTTONDOWN , 0x0001, 1, true)},
			{ ButtonFlags.RI_MOUSE_LEFT_BUTTON_UP , (MouseInputNotifications.WM_LBUTTONUP, 0, 1, false) },

			{ ButtonFlags.RI_MOUSE_RIGHT_BUTTON_DOWN , (MouseInputNotifications.WM_RBUTTONDOWN, 0x0002, 2, true)},
			{ ButtonFlags.RI_MOUSE_RIGHT_BUTTON_UP , (MouseInputNotifications.WM_RBUTTONUP, 0, 2, false) },

			{ ButtonFlags.RI_MOUSE_MIDDLE_BUTTON_DOWN , (MouseInputNotifications.WM_MBUTTONDOWN, 0x0010, 3, true) },
			{ ButtonFlags.RI_MOUSE_MIDDLE_BUTTON_UP , (MouseInputNotifications.WM_MBUTTONUP, 0, 3, false) }
		};

		#region End key
		private static ushort endKey = 0x23;//End
		private static bool WaitingToSetEndKey = false;


		public static void WaitToSetEndKey()
		{
			WaitingToSetEndKey = true;
			Program.Form.SetEndButtonText("Press a key...");
		}

		public static void StopWaitingToSetEndKey()
		{
			WaitingToSetEndKey = false;
			Program.Form.SetEndButtonText($"Stop button = {System.Windows.Input.KeyInterop.KeyFromVirtualKey(endKey)}");
		}
		#endregion

		public static void WndProc(ref Message msg)
		{
			if (msg.Msg == WinApi.WM_INPUT)
			{
				IntPtr hRawInput = msg.LParam;

				Process(hRawInput);
			}
		}

		private static void Process(IntPtr hRawInput)
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
										endKey = rawBuffer.data.keyboard.VKey;
										StopWaitingToSetEndKey();
									}

									LastKeyboardPressed = rawBuffer.header.hDevice;
									break;
								}
							}
							else
							{ 
								if (keyUpOrDown && rawBuffer.data.keyboard.VKey == endKey)//End key
								{
									Console.WriteLine("End key pressed");
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
										if (Options.SendNormalKeyboardInput)
										{
											uint scanCode = rawBuffer.data.keyboard.MakeCode;
											ushort VKey = rawBuffer.data.keyboard.VKey;

											bool keyDown = keyboardMessage == (uint)KeyboardMessages.WM_KEYDOWN;

											uint code = 0x000000000000001 | (scanCode << 16);//32-bit
											if (!keyDown) code |= 0xC0000000;//WM_KEYUP required the bit 31 and 30 to be 1

											uint t = keyDown ? (uint)SendMessageTypes.WM_KEYDOWN : (uint)SendMessageTypes.WM_KEYUP;
											SendInput.WinApi.PostMessageA(hWnd, t, (IntPtr)VKey, (UIntPtr)code);
										}

										//Resend raw input to application. Works for some games only
										if (Options.SendRawKeyboardInput)
											SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0001, (IntPtr)hRawInput);
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
									Program.SplitScreenManager.SetMousePointer(rawBuffer.header.hDevice);
								}
								break; 
							}

							foreach (Window window in Program.SplitScreenManager.GetWindowsForDevice(mouseHandle))
							{
								IntPtr hWnd = window.hWnd;

								//Resend raw input to application. Works for some games only
								if (Options.SendRawMouseInput)
								{
									SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0001, (IntPtr)hRawInput);//TODO: 0 or 1?
								}

								IntVector2 mouseVec = window.MousePosition;

								mouseVec.x = Math.Min(window.Width, Math.Max(mouseVec.x + mouse.lLastX, 0));
								mouseVec.y = Math.Min(window.Height, Math.Max(mouseVec.y + mouse.lLastY, 0));

								//Console.WriteLine($"MOUSE. flags={mouse.usFlags}, X={mouseVec.x}, y={mouseVec.y}, buttonFlags={mouse.usButtonFlags} device pointer = {rawBuffer.header.hDevice}");

								long packedXY = (mouseVec.y * 0x10000) + mouseVec.x;

								//TODO: move away to reduce lag?
								//TODO: BL2 menus work when cursor isn't clipped (it uses the os mouse pointer though)
								Cursor.Position = new System.Drawing.Point(0, 0);
								Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(1, 1));

								if (Options.SendNormalMouseInput)
								{
									ushort mouseMoveState = 0x0000;
									var (l, m, r) = window.MouseState;
									if (l) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_LBUTTON;
									if (m) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_MBUTTON;
									if (r) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_RBUTTON;
									mouseMoveState |= 1 << 7;//Signature for USS 
									SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEMOVE, (IntPtr)mouseMoveState, (IntPtr)packedXY);
									//SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_NCMOUSEMOVE, (IntPtr)0x0000, (IntPtr)packedXY);
								}

								//Mouse buttons.
								ushort f = mouse.usButtonFlags;
								if (f != 0)
								{
									foreach (var pair in ButtonFlagToMouseInputNotifications)
									{
										if ((f & (ushort)pair.Key) > 0)
										{
											var (msg, wParam, leftMiddleRight, isButtonDown) = pair.Value;
											//Console.WriteLine(pair.Key);
											SendInput.WinApi.PostMessageA(hWnd, (uint)msg, (IntPtr)wParam, (IntPtr)packedXY);

											var state = window.MouseState;
											switch (leftMiddleRight)
											{
												case 1:
													state.l = isButtonDown;

													if (Options.RefreshWindowBoundsOnMouseClick)
														window.UpdateBounds();

													break;
												case 2:
													state.m = isButtonDown; break;
												case 3:
													state.r = isButtonDown; break;
											}
											window.MouseState = state;
										}
									}

									if ((f & (ushort)ButtonFlags.RI_MOUSE_WHEEL) > 0)
									{
										ushort delta = mouse.usButtonData;
										SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEWHEEL, (IntPtr)((delta * 0x10000) + 0), (IntPtr)packedXY);
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
