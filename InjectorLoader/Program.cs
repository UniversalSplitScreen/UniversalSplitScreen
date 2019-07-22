using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

//Output file is named IJ not InjectorLoader because "InjectorLoader" is picked up by some antiviruses

namespace InjectorLoader
{
	/* Unused, but shows the layout we use in the byte array
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
	};*/

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
			const int argsLength = 19;

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
			bool HookDInput = nextBool();
			


			const int size = 1024;
			byte[] data = new byte[size];

			byte[] toByteArray(string str)
			{
				List<byte> c = str.ToCharArray().Select(_x => (byte)_x).ToList();
				for (int _i = c.Count; _i < 256; _i++)
					c.Add(0);
				return c.ToArray();
			}

			void writeInt(int num, int offset)
			{
				data[offset]   = (byte)(num >> 24);
				data[offset+1] = (byte)(num >> 16);
				data[offset+2] = (byte)(num >> 8);
				data[offset+3] = (byte)(num);
			}

			void writeBool(bool b, int offset)
			{
				data[offset] = b == true ? (byte)1 : (byte)0;
			}
			
			var str1 = toByteArray(ipcChannelNameRead);
			for (int j = 0; j < str1.Length; j++)
				data[j] = str1[j];

			var str2 = toByteArray(ipcChannelNameWrite);
			for (int j = 0; j < str1.Length; j++)
				data[256 + j] = str2[j];

			writeInt((int)hWnd, 512);
			writeInt(controllerIndex, 516);
			writeInt(allowedMouseHandle, 520);

			int x = 524;
			writeBool(updateAbsoluteFlagInMouseMessage, x++);
			writeBool(HookGetCursorPos, x++);
			writeBool(HookGetForegroundWindow, x++);
			writeBool(HookGetAsyncKeyState, x++);
			writeBool(HookGetKeyState, x++);
			writeBool(HookCallWindowProcW, x++);
			writeBool(HookRegisterRawInputDevices, x++);
			writeBool(HookSetCursorPos, x++);
			writeBool(HookXInput, x++);
			writeBool(useLegacyInput, x++);
			writeBool(hookMouseVisibility, x++);
			writeBool(HookDInput, x++);

			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(data, 0, ptr, size);

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
