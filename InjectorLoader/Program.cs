using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace InjectorLoader
{
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 536)]
	struct UserData
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] ipcChannelNameRead;//Name will be 30 characters

		[FieldOffset(256)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] ipcChannelNameWrite;//Name will be 30 characters

		[FieldOffset(512)]
		public int hWnd;

		//[FieldOffset(4)]
		//[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
		//public string ipcChannelNameRead;

		//[FieldOffset(260)]
		//[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
		//public string ipcChannelNameWrite;

		[FieldOffset(516)]
		public int controllerIndex;

		[FieldOffset(520)]
		public int allowedMouseHandle;

		[FieldOffset(524)]
		public bool updateAbsoluteFlagInMouseMessage;
		[FieldOffset(525)]
		public bool HookGetCursorPos;
		[FieldOffset(526)]
		public bool HookGetForegroundWindow;
		[FieldOffset(527)]
		public bool HookGetAsyncKeyState;
		[FieldOffset(528)]
		public bool HookGetKeyState;
		[FieldOffset(529)]
		public bool HookCallWindowProcW;
		[FieldOffset(530)]
		public bool HookRegisterRawInputDevices;
		[FieldOffset(531)]
		public bool HookSetCursorPos;
		[FieldOffset(532)]
		public bool HookXInput;
		[FieldOffset(533)]
		public bool useLegacyInput;
		[FieldOffset(534)]
		public bool hookMouseVisibility;
	};

	static class Program
	{
		class Injector32
		{
			[DllImport("EasyHook32.dll", CharSet = CharSet.Ansi)]
			public static extern int RhInjectLibrary(
				uint InTargetPID,
				uint InWakeUpTID,
				uint InInjectionOptions,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x86,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x64,
				IntPtr InPassThruBuffer,
				uint InPassThruSize
				);
		}

		class Injector64
		{
			[DllImport("EasyHook64.dll", CharSet = CharSet.Ansi)]
			public static extern int RhInjectLibrary(
				uint InTargetPID,
				uint InWakeUpTID,
				uint InInjectionOptions,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x86,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x64,
				IntPtr InPassThruBuffer,
				uint InPassThruSize
				);
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
			/*uint nt;
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
			}*/

			byte[] toByteArray(string str)
			{
				List<byte> c = str.ToCharArray().Select(x => (byte)x).ToList();
				for (int _i = c.Count; _i < 256; _i++)
					c.Add(0);
				return c.ToArray();
			}

			UserData data = new UserData()
			{
				hWnd = (int)hWnd,
				ipcChannelNameRead = toByteArray(ipcChannelNameRead),
				ipcChannelNameWrite = toByteArray(ipcChannelNameWrite),
				//ipcChannelNameRead = ipcChannelNameRead,
				//ipcChannelNameWrite = ipcChannelNameWrite,
				controllerIndex = controllerIndex,
				allowedMouseHandle = allowedMouseHandle,
				updateAbsoluteFlagInMouseMessage = updateAbsoluteFlagInMouseMessage,

				HookGetCursorPos = HookGetCursorPos,
				HookGetForegroundWindow = HookGetForegroundWindow,
				HookGetAsyncKeyState = HookGetAsyncKeyState,
				HookGetKeyState = HookGetKeyState,
				HookCallWindowProcW = HookCallWindowProcW,
				HookRegisterRawInputDevices = HookRegisterRawInputDevices,
				HookSetCursorPos = HookSetCursorPos,
				HookXInput = HookXInput,
				useLegacyInput = useLegacyInput,
				hookMouseVisibility = hookMouseVisibility
			};

			int size = Marshal.SizeOf(data);//538

			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(data, ptr, false);

			int nt = 0;

			if (Environment.Is64BitProcess)
				nt = Injector64.RhInjectLibrary((uint)pid, 0, 0, "", injectionDllPath, ptr, (uint)size);
			else
				nt = Injector32.RhInjectLibrary((uint)pid, 0, 0, injectionDllPath, "", ptr, (uint)size);

			//Set exit code
			Environment.Exit((int)nt);
		}
	}
}
