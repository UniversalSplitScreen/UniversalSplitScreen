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
			IntPtr allowed = _server.GetAllowed_hRawInput();
			if (allowed == hRawInput)
			{
				return GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);
			}

			pData = default(RAWINPUT);
			//return 0xFFFFFFFF; (TODO)
			return 0;
		}
		#endregion

		#region GetCursorPos hook
		private static EasyHook.LocalHook getCursorPosHook;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT lpPoint);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate bool GetCursorPosDelegate(out POINT lpPoint);

		public bool GetCursorPosHook(out POINT lpPoint)
		{
			_server.ReportMessage("GetCursorPos called");
			lpPoint = new POINT();
			return false;
			//return GetCursorPos(out lpPoint);
			/*POINT p = new POINT();
			p.x = 200;
			p.y = 200;

			lpPoint = p;
			
			return true;*/
		}
		#endregion

		#region SDL MouseGetGlobalState hook
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
			_server.ReportMessage("SDL get mouse state called");
			x = 250;
			y = 250;
			return 0;
		}

		public IntPtr SdlGetMouseHook()
		{
			_server.ReportMessage("SDL get mouse called");
			return IntPtr.Zero;
		}
		#endregion

		#region GetForegroundWindow hook
		private static EasyHook.LocalHook getForegroundWindowHook;

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public IntPtr GetForegroundWindowHook()
		{
			//_server.ReportMessage("GetForegroundWindowHook called");

			//IntPtr actual = GetForegroundWindow();

			//IntPtr actual = GetForegroundWindow();
			//_server.ReportMessage($"game={hWnd}, actual={actual}");

			return hWnd;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr GetForegroundWindowDelegate();
		#endregion

		ServerInterface _server = null;
		
		public InjectionEntryPoint(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			_server = EasyHook.RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
			_server.Ping();
		}

		public void Run(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			int pid = EasyHook.RemoteHooking.GetCurrentProcessId();
			_server.Ping();
			//_server.IsInstalled(pid);

			hWnd = _server.GetGame_hWnd();
			//_server.ReportMessage($"hWnd for hook = {hWnd}");

			try
			{
				#region off
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
				#endregion


				//TODO: make unmanaged for better performance
				/*getRawInputDataHook = EasyHook.LocalHook.Create(
						EasyHook.LocalHook.GetProcAddress("user32.dll", "GetRawInputData"),
						new GetRawInputDataDelegate(GetRawInputDataHook),
						this);*/

				getRawInputDataHook = EasyHook.LocalHook.CreateUnmanaged(
						EasyHook.LocalHook.GetProcAddress("user32.dll", "GetRawInputData"),
						Marshal.GetFunctionPointerForDelegate(new GetRawInputDataDelegate(GetRawInputDataHook)),
						IntPtr.Zero);


				/*getForegroundWindowHook = EasyHook.LocalHook.Create(
					EasyHook.LocalHook.GetProcAddress("user32.dll", "GetForegroundWindow"),
					new GetForegroundWindowDelegate(GetForegroundWindowHook),
					this);*/



				getForegroundWindowHook = EasyHook.LocalHook.CreateUnmanaged(
					EasyHook.LocalHook.GetProcAddress("user32.dll", "GetForegroundWindow"),
					Marshal.GetFunctionPointerForDelegate(new GetForegroundWindowDelegate(GetForegroundWindowHook)),
					IntPtr.Zero);


				getRawInputDataHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
				getForegroundWindowHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });



				//_server.ReportMessage($"Installed GetRawInputData hook on {pid}");

				//EasyHook.RemoteHooking.WakeUpProcess();//TODO: is this required?
			}
			catch(Exception e)
			{
				_server.ReportMessage($"ERROR INSTALLING HOOK: {e.ToString()}");
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
			getCursorPosHook?.Dispose();
			sdlMouseGetGlobalStateHook?.Dispose();
			getForegroundWindowHook?.Dispose();

			EasyHook.LocalHook.Release();
		}

		
	}
}
