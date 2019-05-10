using System;
using System.Runtime.InteropServices;

namespace InjectorLoader64
{
	static class Program
	{
		[DllImport("InjectorCPP64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern uint Inject(
				int pid,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
				IntPtr hWnd,
				string ipcChannelName,
				bool HookGetCursorPos,
				bool HookGetForegroundWindow,
				bool HookGetAsyncKeyState,
				bool HookGetKeyState,
				bool HookCallWindowProcW,
				bool HookRegisterRawInputDevices,
				bool HookSetCursorPos);

		public static void Main(string[] args)
		{
			if (args.Length != 11)
			{
				throw new ArgumentException("Need exactly 11 arguments");
			}

			//Arguments
			int.TryParse(args[0], out int pid);

			string injectionDllPath = args[1];

			int.TryParse(args[2], out int _hWnd);
			IntPtr hWnd = (IntPtr)_hWnd;

			string ipcChannelName = args[3];

			int i = 4;
			bool nextBool() => args[i++].ToLower().Equals("true");

			bool HookGetCursorPos = nextBool();
			bool HookGetForegroundWindow = nextBool();
			bool HookGetAsyncKeyState = nextBool();
			bool HookGetKeyState = nextBool();
			bool HookCallWindowProcW = nextBool();
			bool HookRegisterRawInputDevices = nextBool();
			bool HookSetCursorPos = nextBool();

			//InjectorCPP64 function
			uint nt = Inject(pid,
				"",
				injectionDllPath,
				hWnd,
				ipcChannelName,
				HookGetCursorPos,
				HookGetForegroundWindow,
				HookGetAsyncKeyState,
				HookGetKeyState,
				HookCallWindowProcW,
				HookRegisterRawInputDevices,
				HookSetCursorPos);

			//Set exit code
			Environment.Exit((int)nt);
		}
	}
}
