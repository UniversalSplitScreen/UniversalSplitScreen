using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GetRawInputDataHook
{
	public class InjectionEntryPoint : EasyHook.IEntryPoint
	{
		private static IntPtr hWnd = IntPtr.Zero;

		ServerInterface _server = null;

		IntPtr allowedRawInputDevice = IntPtr.Zero;//TODO: remove (unused)

		#region GetRawInputData hook
		private static EasyHook.LocalHook getRawInputDataHook;

		//[DllImport("user32.dll")]
		//static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out IntPtr pData, ref uint pcbSize, int cbSizeHeader);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate uint GetRawInputDataDelegate(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		[DllImport("user32.dll")]
		public static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		int x = 0;
		public uint GetRawInputDataHook(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader)
		{
			//TODO: remove try catch to improve performance?

			if (x < 15)
			{
				try
				{
					RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];

					//Mouse
					rid[0].usUsagePage = 0x01;
					rid[0].usUsage = 0x02;
					rid[0].dwFlags = (uint)0x00000001;
					rid[0].hwndTarget = (IntPtr)null;
					//rid[0].hwndTarget = hWnd;

					bool success = RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0]));//TODO: UNREGISTER RAW KEYBOARD???? (for bl2)
					ReportMessage($"unregister success = {success}");
					x++;
				}
				catch (Exception e)
				{
					ReportMessage($"unregister error = {e}");
				}
			}

			ReleaseGetRawInputDataHook();
			return GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);//TODO: REMOVE?

			/*
			#region Works in BL2, not GMod
			//run the function, check if it is the allowed device, then pass through or not
			GetRawInputData(hRawInput, uiCommand, out RAWINPUT ri, ref pcbSize, cbSizeHeader);

			if (ri.header.hDevice == allowedRawInputDevice)
			{
				return GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);
			}
			else
			{
				GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);
				return 0xFFFFFFFF;//Gmod/source ignores this error message
			}
			#endregion

			//TODO: for gmod, do pData = default(RAWINPUT) when ignoring a message. (Crashes BL2)
			*/
		}
		#endregion

		#region GetCursorPos hook
		//TODO: test with monogame games? (MonoGame uses SDL)
		private static EasyHook.LocalHook getCursorPosHook;
		
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		public delegate bool GetCursorPosDelegate(out POINT lpPoint);
		
		public bool GetCursorPosHook(out POINT lpPoint)
		{
			_server.GetCursorPosition(out int x, out int y);

			POINT p = new POINT
			{
				x = x,
				y = y
			};

			ClientToScreen(hWnd, ref p);

			lpPoint = p;

			return true;
		}

		#endregion

		
		#region GetAsyncKeyState hook
		private static EasyHook.LocalHook getAsyncKeyStateHook;

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int vKey);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		public delegate short GetAsyncKeyStateDelegate(int vKey);

		public short GetAsyncKeyStateHook(int vKey)
		{
			return (short)(_server.GetIsVKeyPressed(vKey) ? (1 << 15) : 0); ;
		}
		#endregion

		#region GetKeyState hook
		private static EasyHook.LocalHook getKeyStateHook;

		[DllImport("user32.dll")]
		public static extern short GetKeyState(int nVirtKey);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		public delegate short GetKeyStateDelegate(int nVirtKey);

		public short GetKeyStateHook(int nVirtKey)
		{
			if (nVirtKey == 0x41 || nVirtKey == 0x44 || nVirtKey == 0x53 || nVirtKey == 0x57)//WASD
				return _server.GetIsVKeyPressed(nVirtKey) ? (short)-32768 : (short)0;//-32768: Key down (aka high order bit = 1)
			else
				return GetKeyState(nVirtKey);
		}
		#endregion


		/* GetKeyboardState Hook: unnecessary as posting keyboard input to message queue already affects GetKeyboardState
		#region GetKeyboardState Hook
		//TODO: make configurable
		private static EasyHook.LocalHook getKeyboardStateHook;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetKeyboardState(IntPtr lpKeyState);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		public delegate bool GetKeyboardStateDelegate(out IntPtr lpKeyState);

		public bool GetKeyboardStateHook(out IntPtr lpKeyState)
		{
			IntPtr ptr = Marshal.AllocHGlobal(256 * sizeof(byte));
			//GetKeyboardState(ptr);

			byte[] kbBytes = new byte[256];

			//Marshal.Copy(ptr, kbBytes, 0, 256);
			Marshal.Copy(kbBytes, 0, ptr, 256);
			
			

			for (int i = 0; i < 256; i++)
			{
				if (kbBytes[i] != 0)
					_server.ReportMessage($"i={i}, byte={kbBytes[i]}");
			}
			//lpKeyState = kbBytes;

			lpKeyState = ptr;
			return true;
		}


		#endregion
		*/

		/*SDL MouseGetGlobalState Hook
		#region SDL MouseGetGlobalState Hook
		private static EasyHook.LocalHook sdlMouseGetGlobalStateHook;

		[Flags]
		public enum Button
		{
			Left = 1 << 0,
			Middle = 1 << 1,
			Right = 1 << 2,
			X1Mask = 1 << 3,
			X2Mask = 1 << 4
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate UInt32 d_sdl_getglobalmousestate(out int x, out int y);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate IntPtr d_sdl_getmouse();

		public UInt32 SdlGetGlobalMouseStateHook(out int x, out int y)
		{
			ReportMessage("SDL get mouse state called");
			x = 250;
			y = 250;
			return 0;
		}

		public IntPtr SdlGetMouseHook()
		{
			ReportMessage("SDL get mouse called");
			return IntPtr.Zero;
		}
		#endregion
		*/

		#region GetForegroundWindow hook
		//Used by BL2 and starbound
		private static EasyHook.LocalHook getForegroundWindowHook;

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public IntPtr GetForegroundWindowHook()
		{
			return hWnd;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr GetForegroundWindowDelegate();
		#endregion

		#region CallWindowProc hook

		private static EasyHook.LocalHook callWindowProcHook;

		public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern IntPtr CallWindowProc(WndProcDelegate lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr CallWindowProcDelegate(WndProcDelegate lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public IntPtr CallWindowProcHook(WndProcDelegate lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
		{
			try
			{
				//USS signature is 1 << 7 or 0b10000000 for WM_MOUSEMOVE(0x0200). If this is detected, allow event to pass
				if (Msg == 0x0200 && ((int)wParam & 0b10000000) > 0)
					return CallWindowProc(lpPrevWndFunc, hWnd, Msg, wParam, lParam);

				// || Msg == 0x00FF
				else if ((Msg >= 0x020B && Msg <= 0x020D) || Msg == 0x0200 || Msg == 0x0021 || Msg == 0x02A1 || Msg == 0x02A3)//Other mouse events. 
					return IntPtr.Zero;
				else
				{
					if (false && Msg == 0x0006) //0x0006 is WM_ACTIVATE, which resets the mouse position for starbound [citation needed]
 						return CallWindowProc(lpPrevWndFunc, hWnd, Msg, (IntPtr)1, (IntPtr)0);
					else
						return CallWindowProc(lpPrevWndFunc, hWnd, Msg, wParam, lParam);
				}
			}
			catch(Exception e)
			{
				ReportMessage($"Error in CallWindowProcHook: {e.ToString()}");
				return CallWindowProc(lpPrevWndFunc, hWnd, Msg, wParam, lParam);
			}
		}

		#endregion

		#region RegisterRawInputDevices hook
		//TODO: MAKE CONFIGURABLE

		private static EasyHook.LocalHook registerRawInputDevicesHook;

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-registerrawinputdevices
		[DllImport("User32.dll")]
		public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate bool RegisterRawInputDevicesDelegate(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

		public bool RegisterRawInputDevicesHook(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize)
		{
			return true;//Pretend it succeeded
		}
		#endregion

		/* GetFocus Hook
		#region GetFocus hook
		private static EasyHook.LocalHook getFocusHook;

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getfocus
		[DllImport("User32.dll")]
		public static extern IntPtr GetFocus();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr GetFocusDelegate();
		
		public IntPtr GetFocusHook()
		{
			return hWnd;
		}
		#endregion
		*/

		/* GetActiveWindow Hook
		#region GetActiveWindow hook
		private static EasyHook.LocalHook getActiveWindowHook;

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getactivewindow
		[DllImport("User32.dll")]
		public static extern IntPtr GetActiveWindow();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr GetActiveWindowDelegate();
		
		public IntPtr GetActiveWindowHook()
		{
			return hWnd;
		}
		#endregion
		*/

		

		public InjectionEntryPoint(EasyHook.RemoteHooking.IContext context, string channelName, bool hookRawInput, bool hookCallWndProc, bool hookGetForegroundWindow)
		{
			_server = EasyHook.RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
			_server.Ping();
		}

		public void Run(EasyHook.RemoteHooking.IContext context, string channelName, bool hookRawInput, bool hookCallWndProc, bool hookGetForegroundWindow)
		{
			int pid = EasyHook.RemoteHooking.GetCurrentProcessId();
			_server.IsInstalled(pid);

			hWnd = _server.GetGame_hWnd();
			allowedRawInputDevice = _server.GetAllowed_hDevice();
			ReportMessage($"InjectionEntryPoint: hWnd={hWnd}, rid={allowedRawInputDevice}");
			
			try
			{
				#region disabled
				/*getCursorPosHook = EasyHook.LocalHook.Create(
						EasyHook.LocalHook.GetProcAddress("user32.dll", "GetCursorPos"),
						new GetCursorPosDelegate(GetCursorPosHook),
						this);

				getCursorPosHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });*/

				/*
				//string sdlPath = @"D:\Steam\steamapps\common\GarrysMod\bin\SDL2.dll";
				//string sdlPath = @"G:\SteamGames2\steamapps\common\Starbound\win64\SDL2.dll";

				//IntPtr SDLPtr = LoadLibraryW(sdlPath);
				//ReportMessage($"SDL ptr = {SDLPtr}");

				IntPtr procAddr = EasyHook.LocalHook.GetProcAddress("SDL2.dll", "SDL_GetGlobalMouseState");
				ReportMessage($"SDL proc addr = {procAddr}");

				sdlMouseGetGlobalStateHook = EasyHook.LocalHook.Create(
						procAddr,
						new d_sdl_getglobalmousestate(SdlGetGlobalMouseStateHook),
						this);

				sdlMouseGetGlobalStateHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });*/

				/*getCursorPosHook = EasyHook.LocalHook.CreateUnmanaged(
						EasyHook.LocalHook.GetProcAddress("user32.dll", "GetCursorPos"),
						Marshal.GetFunctionPointerForDelegate(new GetCursorPosDelegate(GetCursorPosHook)),
						IntPtr.Zero);*/

				//getCursorPosHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
				#endregion

				EasyHook.LocalHook CreateHook(string InModule, string InSymbolName, Delegate dele)
				{
					//TODO: make checkbox for Create/CreateUnmanaged (CreateUnmanaged crashes XNA games)
					/*var x = EasyHook.LocalHook.CreateUnmanaged(
								EasyHook.LocalHook.GetProcAddress(InModule, InSymbolName),
								Marshal.GetFunctionPointerForDelegate(dele),
								IntPtr.Zero);*/

					var x = EasyHook.LocalHook.Create(
								EasyHook.LocalHook.GetProcAddress(InModule, InSymbolName),
								dele,
								this);

					x.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

					return x;
				}

				
				getCursorPosHook = CreateHook("user32.dll", "GetCursorPos", new GetCursorPosDelegate(GetCursorPosHook));
				ReportMessage($"Hooked GetCursorPos on {pid}");

				getAsyncKeyStateHook = CreateHook("user32.dll", "GetAsyncKeyState", new GetAsyncKeyStateDelegate(GetAsyncKeyStateHook));
				ReportMessage($"Hooked GetAsyncKeyState on {pid}");

				getKeyStateHook = CreateHook("user32.dll", "GetKeyState", new GetKeyStateDelegate(GetKeyStateHook));
				ReportMessage($"Hooked GetKeyState on {pid}");

				if (hookRawInput)
				{
					getRawInputDataHook = CreateHook("user32.dll", "GetRawInputData", new GetRawInputDataDelegate(GetRawInputDataHook));
					registerRawInputDevicesHook = CreateHook("user32.dll", "RegisterRawInputDevices", new RegisterRawInputDevicesDelegate(RegisterRawInputDevicesHook));

					ReportMessage($"Hooked GetRawInputData and RegisterRawInputDevices on {pid}");
				}

				if (hookGetForegroundWindow)
				{
					getForegroundWindowHook = CreateHook("user32.dll", "GetForegroundWindow", new GetForegroundWindowDelegate(GetForegroundWindowHook));

					//TODO: allow these to be activated in their own separate options checkbox
					//getFocusHook = CreateHook("user32.dll", "GetFocus", new GetFocusDelegate(GetFocusHook));
					//getActiveWindowHook = CreateHook("user32.dll", "GetActiveWindow", new GetActiveWindowDelegate(GetActiveWindowHook));


					ReportMessage($"Hooked GetForegroundWindow on {pid}");
				}

				if (hookCallWndProc)
				{
					callWindowProcHook = CreateHook("user32.dll", "CallWindowProcW", new CallWindowProcDelegate(CallWindowProcHook));
					ReportMessage($"Hooked CallWindowProcW on on {pid}");
				}
			}
			catch (Exception e)
			{
				ReportMessage($"Error installing hook: {e.ToString()}");
			}
		


			try
			{
				while (true)
				{
					System.Threading.Thread.Sleep(1000);
					_server.Ping();

					if (_server.ShouldReleaseHook())
						break;
				}
			}
			catch (Exception)
			{

			}

			ReleaseHooks();
		}

		private void ReleaseGetRawInputDataHook()
		{
			ReportMessage("Releasing GetRawInputData hook");
			getRawInputDataHook?.Dispose();
		}

		private void ReleaseHooks()
		{
			ReportMessage("Releasing hooks");

			ReleaseGetRawInputDataHook();
			getForegroundWindowHook?.Dispose();
			callWindowProcHook?.Dispose();
			//getActiveWindowHook?.Dispose();
			//getFocusHook?.Dispose();

			getCursorPosHook?.Dispose();
			getAsyncKeyStateHook?.Dispose();
			getKeyStateHook?.Dispose();

			EasyHook.LocalHook.Release();
		}

		private void ReportMessage(string message)
		{
			try
			{
				_server.ReportMessage(message);
			}
			catch (Exception)
			{

			}
		}
	}
}
