using System;
using System.Runtime.InteropServices;

namespace InjectorLoader
{
	static class Program
	{
		class Injector32
		{
			[DllImport("InjectorCPP32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern uint Inject(
					int pid,
					[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
					[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
					IntPtr hWnd,
					string ipcChannelNameRead,
					string ipcChannelNameWrite,
					int controllerIndex,
					int allowedMouseHandle,
					bool updateAbsoluteFlagInMouseMessage,
					bool useLegacyInput,
					bool HookGetCursorPos,
					bool HookGetForegroundWindow,
					bool HookGetAsyncKeyState,
					bool HookGetKeyState,
					bool HookCallWindowProcW,
					bool HookRegisterRawInputDevices,
					bool HookSetCursorPos,
					bool HookXInput,
					bool hookMouseVisibility);
		}

		class Injector64
		{
			[DllImport("InjectorCPP64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern uint Inject(
					int pid,
					[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
					[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
					IntPtr hWnd,
					string ipcChannelNameRead,
					string ipcChannelNameWrite,
					int controllerIndex,
					int allowedMouseHandle,
					bool updateAbsoluteFlagInMouseMessage,
					bool useLegacyInput,
					bool HookGetCursorPos,
					bool HookGetForegroundWindow,
					bool HookGetAsyncKeyState,
					bool HookGetKeyState,
					bool HookCallWindowProcW,
					bool HookRegisterRawInputDevices,
					bool HookSetCursorPos,
					bool HookXInput,
					bool hookMouseVisibility);
		}

		public static void Main(string[] args)
		{
			const int argsLength = 18;

			if (args.Length != argsLength)
			{
				throw new ArgumentException($"Need exactly {argsLength} arguments");
			}

			//Arguments
			int i = 0;
			int.TryParse(args[i++], out int pid);

			string injectionDllPath = args[i++];

			int.TryParse(args[i++], out int _hWnd);
			IntPtr hWnd = (IntPtr)_hWnd;

			string ipcChannelNameRead = args[i++];
			string ipcChannelNameWrite = args[i++];

			int.TryParse(args[i++], out int controllerIndex);

			int.TryParse(args[i++], out int allowedMouseHandle);

			bool updateAbsoluteFlagInMouseMessage = args[i++].ToLower().Equals("true");

			bool useLegacyInput = args[i++].ToLower().Equals("true");
			
			bool nextBool() => args[i++].ToLower().Equals("true");

			bool HookGetCursorPos = nextBool();
			bool HookGetForegroundWindow = nextBool();
			bool HookGetAsyncKeyState = nextBool();
			bool HookGetKeyState = nextBool();
			bool HookCallWindowProcW = nextBool();
			bool HookRegisterRawInputDevices = nextBool();
			bool HookSetCursorPos = nextBool();
			bool HookXInput = nextBool();
			bool hookMouseVisibility = nextBool();

			//InjectorCPP function
			uint nt;
			if (Environment.Is64BitProcess)
			{
				nt = Injector64.Inject(pid,
					"",
					injectionDllPath,
					hWnd,
					ipcChannelNameRead,
					ipcChannelNameWrite,
					controllerIndex,
					allowedMouseHandle,
					updateAbsoluteFlagInMouseMessage,
					useLegacyInput,
					HookGetCursorPos,
					HookGetForegroundWindow,
					HookGetAsyncKeyState,
					HookGetKeyState,
					HookCallWindowProcW,
					HookRegisterRawInputDevices,
					HookSetCursorPos,
					HookXInput,
					hookMouseVisibility);
			}
			else
			{
				nt = Injector32.Inject(pid,
					injectionDllPath,
					"",
					hWnd,
					ipcChannelNameRead,
					ipcChannelNameWrite,
					controllerIndex,
					allowedMouseHandle,
					updateAbsoluteFlagInMouseMessage,
					useLegacyInput,
					HookGetCursorPos,
					HookGetForegroundWindow,
					HookGetAsyncKeyState,
					HookGetKeyState,
					HookCallWindowProcW,
					HookRegisterRawInputDevices,
					HookSetCursorPos,
					HookXInput,
					hookMouseVisibility);
			}

			//Set exit code
			Environment.Exit((int)nt);
		}
	}
}
