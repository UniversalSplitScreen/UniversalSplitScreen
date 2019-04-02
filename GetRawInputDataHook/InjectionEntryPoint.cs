using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GetRawInputDataHook
{
	public class InjectionEntryPoint : EasyHook.IEntryPoint
	{
		private static IntPtr hWnd = IntPtr.Zero;

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr LoadLibraryW(string lpszLib);

		#region GetRawInputData hook
		private static EasyHook.LocalHook getRawInputDataHook;

		//[DllImport("user32.dll")]
		//static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out IntPtr pData, ref uint pcbSize, int cbSizeHeader);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate uint GetRawInputDataDelegate(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		[DllImport("user32.dll")]
		public static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		public uint GetRawInputDataHook(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader)
		{
			//TODO: remove try catch to improve performance?
			try
			{

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
			}
			catch(Exception e)
			{
				_server.ReportMessage($"error in GetRawInputDataHook: {e.ToString()}");
				return GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);
			}
		}
		#endregion

		#region GetCursorPos hook
		private static EasyHook.LocalHook getCursorPosHook;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos([Out] IntPtr lpPoint);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate bool GetCursorPosDelegate(out IntPtr lpPoint);

		public bool GetCursorPosHook(out IntPtr lpPoint)
		{
			_server.ReportMessage("GetCursorPos called");
			//GetCursorPos(out lpPoint);
			POINT p = new POINT();
			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(POINT)));
			Marshal.StructureToPtr(p, ptr, false);

			lpPoint = ptr;

			return false;
			//return GetCursorPos(out lpPoint);
			//POINT p = new POINT();
			//p.x = 200;
			//p.y = 200;

			//lpPoint = p;
			
			//return true;
		}
		#endregion

		#region SDL MouseGetGlobalState hook
		/*private static EasyHook.LocalHook sdlMouseGetGlobalStateHook;
		
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
			_server.ReportMessage("SDL get mouse state called");
			x = 250;
			y = 250;
			return 0;
		}

		public IntPtr SdlGetMouseHook()
		{
			_server.ReportMessage("SDL get mouse called");
			return IntPtr.Zero;
		}*/
		#endregion

		#region GetForegroundWindow hook
		//Used by BL2 and starbound
		private static EasyHook.LocalHook getForegroundWindowHook;

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public IntPtr GetForegroundWindowHook()
		{
			try
			{
				return hWnd;
			}
			catch (Exception)
			{
				_server.ReportMessage("Error in GetForegroundWindowHook");
				return GetForegroundWindow();
			}
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
				else if ((Msg >= 0x020B && Msg <= 0x020D) || Msg == 0x0200 || Msg == 0x0021 || Msg == 0x02A1 || Msg == 0x00FF || Msg == 0x02A3)//Other mouse events. 
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
				_server.ReportMessage($"Error in CallWindowProcHook: {e.ToString()}");
				return CallWindowProc(lpPrevWndFunc, hWnd, Msg, wParam, lParam);
			}
		}

		#endregion

		ServerInterface _server = null;

		IntPtr allowedRawInputDevice = IntPtr.Zero;

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
				//_server.ReportMessage($"SDL ptr = {SDLPtr}");

				IntPtr procAddr = EasyHook.LocalHook.GetProcAddress("SDL2.dll", "SDL_GetGlobalMouseState");
				_server.ReportMessage($"SDL proc addr = {procAddr}");

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
					var x = EasyHook.LocalHook.CreateUnmanaged(
							EasyHook.LocalHook.GetProcAddress(InModule, InSymbolName),
							Marshal.GetFunctionPointerForDelegate(dele),
							IntPtr.Zero);

					x.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

					return x;
				}

				if (hookRawInput)
				{
					getRawInputDataHook = CreateHook("user32.dll", "GetRawInputData", new GetRawInputDataDelegate(GetRawInputDataHook));
				}

				if (hookGetForegroundWindow)
				{
					getForegroundWindowHook = CreateHook("user32.dll", "GetForegroundWindow", new GetForegroundWindowDelegate(GetForegroundWindowHook));
				}

				if (hookCallWndProc)
				{
					callWindowProcHook = CreateHook("user32.dll", "CallWindowProcW", new CallWindowProcDelegate(CallWindowProcHook));
				}

				_server.ReportMessage($"Installed GetRawInputData hook on {pid}");
			}
			catch(Exception e)
			{
				_server.ReportMessage($"Error installing hook: {e.ToString()}");
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
			catch
			{

			}

			ReleaseHooks();
		}

		private void ReleaseHooks()
		{
			_server.ReportMessage("Releasing GetRawInputData hook");

			getRawInputDataHook?.Dispose();
			//getCursorPosHook?.Dispose();
			//sdlMouseGetGlobalStateHook?.Dispose();
			getForegroundWindowHook?.Dispose();
			callWindowProcHook?.Dispose();

			EasyHook.LocalHook.Release();
		}

		
	}
}
