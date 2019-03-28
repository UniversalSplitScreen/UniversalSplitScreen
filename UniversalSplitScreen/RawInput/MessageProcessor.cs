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
	class MessageProcessor
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

		readonly static Dictionary<IntPtr, (bool l, bool m, bool r)> mouseStates = new Dictionary<IntPtr, (bool, bool, bool)> ();

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

							if (keyUpOrDown && 0x23 == rawBuffer.data.keyboard.VKey)//End key
							{
								Console.WriteLine("End key pressed");
								Program.SplitScreenManager.DeactivateSplitScreen();
								InputDisabler.Unlock();//Just in case
							}

							if (keyUpOrDown && !Program.SplitScreenManager.IsRunningInSplitScreen)
							{
								LastKeyboardPressed = rawBuffer.header.hDevice;
								break;
							}

							//Console.WriteLine($"KEYBOARD. key={rawBuffer.data.keyboard.VKey:x}, message = {rawBuffer.data.keyboard.Message:x}, device pointer = {rawBuffer.header.hDevice}");

							#region Get game hWnd
							IntPtr hWnd = new IntPtr(0);
							Window window = null;

							foreach (var w in Program.SplitScreenManager.windows.Values)
							{
								if (w.KeyboardAttached == rawBuffer.header.hDevice)
								{
									hWnd = w.hWnd;
									window = w;
									break;
								}
							}

							if ((int)hWnd == 0) return;
							#endregion

							//TODO
							/*#region Allow in hook
							Core.WinApi.GetWindowThreadProcessId(hWnd, out int _pid);
							var pid = (IntPtr)_pid;
							var d = GetRawInputDataHook.InjectionEntryPoint.allowed_hRawInput_handlesFor_pids;
							if (!d.ContainsKey(pid)) d.Add(pid, new List<IntPtr>());
							d[pid].Add(hRawInput);
							#endregion*/

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

							break;
						}
					case HeaderDwType.RIM_TYPEMOUSE:
						{
							RAWMOUSE mouse = rawBuffer.data.mouse;
							IntPtr mousePtr = rawBuffer.header.hDevice;

							if (!Program.SplitScreenManager.IsRunningInSplitScreen)
							{
								if ((mouse.usButtonFlags & (ushort)ButtonFlags.RI_MOUSE_LEFT_BUTTON_UP) > 0 && Program.Form.ButtonPressed)
								{
									Console.WriteLine($"Set mouse, pointer = {rawBuffer.header.hDevice}");
									Program.SplitScreenManager.SetMousePointer(rawBuffer.header.hDevice);
								}
								break;
							}

							#region Get game hWnd and Window
							IntPtr hWnd = new IntPtr(0);
							Window window = null;

							foreach (var w in Program.SplitScreenManager.windows.Values)
							{
								if (w.MouseAttached == rawBuffer.header.hDevice)
								{
									hWnd = w.hWnd;
									window = w;
									break;
								}
							}

							if ((int)hWnd == 0) return;
							#endregion
							
							//Allow input
							Program.SplitScreenManager.GetRawInputDataHookServer.SetAllowed_hRawInput(hRawInput);

							//Resend raw input to application. Works for some games only
							if (Options.SendRawMouseInput)
							{

								SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0001, (IntPtr)hRawInput);//TODO: 0 or 1?
								//SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_INPUT, (IntPtr)0x0001, (IntPtr)IntPtr.Zero);
							}

							IntVector2 mouseVec = window.MousePosition;

							mouseVec.x += mouse.lLastX;
							mouseVec.x = Math.Min(window.Width, Math.Max(mouseVec.x, 0));
							mouseVec.y += mouse.lLastY;
							mouseVec.y = Math.Min(window.Height, Math.Max(mouseVec.y, 0));

							//Console.WriteLine($"MOUSE. flags={mouse.usFlags}, X={mouseVec.x}, y={mouseVec.y}, buttonFlags={mouse.usButtonFlags} device pointer = {rawBuffer.header.hDevice}");

							long packedXY = (mouseVec.y * 0x10000) + mouseVec.x;

							//TODO: move away to reduce lag?
							Cursor.Position = new System.Drawing.Point(0, 0);
							Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(1, 1));

							if (Options.SendNormalMouseInput)
							{
								ushort mouseMoveState = 0x0000;
								if (mouseStates.TryGetValue(mousePtr, out var o))
								{
									if (o.l) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_LBUTTON;
									if (o.m) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_MBUTTON;
									if (o.r) mouseMoveState |= (ushort)WM_MOUSEMOVE_wParam.MK_RBUTTON;
								}
								SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEMOVE, (IntPtr)mouseMoveState, (IntPtr)packedXY);
								//SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_NCMOUSEMOVE, (IntPtr)0x0000, (IntPtr)packedXY);
							}

							if (!mouseStates.ContainsKey(mousePtr))
								mouseStates[mousePtr] = (false, false, false);

							//Mouse buttons.
							ushort f = mouse.usButtonFlags;
							foreach (var pair in ButtonFlagToMouseInputNotifications)
							{
								if ((f & (ushort)pair.Key) > 0)
								{
									var v = pair.Value;
									//Console.WriteLine(pair.Key);
									SendInput.WinApi.PostMessageA(hWnd, (uint)v.msg, (IntPtr)v.wParam, (IntPtr)packedXY);

									var state = mouseStates[mousePtr];
									switch (v.leftMiddleRight)
									{
										case 1:
											state.l = v.isButtonDown;

											if (Options.RefreshWindowBoundsOnMouseClick)
												window.UpdateBounds();

											break;
										case 2:
											state.m = v.isButtonDown; break;
										case 3:
											state.r = v.isButtonDown; break;
									}
									mouseStates[mousePtr] = state;
								}
							}

							if ((f & (ushort)ButtonFlags.RI_MOUSE_WHEEL) > 0)
							{
								ushort delta = mouse.usButtonData;
								SendInput.WinApi.PostMessageA(hWnd, (uint)MouseInputNotifications.WM_MOUSEWHEEL, (IntPtr)((delta * 0x10000) + 0), (IntPtr)packedXY);
							}

							break;
						}
					default: break;//HID
				}
			}

		}
	}
}
