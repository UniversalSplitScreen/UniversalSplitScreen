using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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


			/*EASYHOOK_NT_EXPORT RhCreateAndInject(
				WCHAR* InEXEPath,
				WCHAR* InCommandLine,
				ULONG InProcessCreationFlags, (documentation is missing this field)
				ULONG InInjectionOptions,
				WCHAR* InLibraryPath_x86,
				WCHAR* InLibraryPath_x64,
				PVOID InPassThruBuffer,
				ULONG InPassThruSize,
				ULONG* OutProcessId);*/
			[DllImport("EasyHook32.dll", CharSet = CharSet.Ansi)]
			public static extern int RhCreateAndInject(
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InEXEPath,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InCommandLine,
				uint InProcessCreationFlags,
				uint InInjectionOptions,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x86,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x64,
				IntPtr InPassThruBuffer,
				uint InPassThruSize,
				IntPtr OutProcessId //Pointer to a UINT (the PID of the new process)
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

			[DllImport("EasyHook64.dll", CharSet = CharSet.Ansi)]
			public static extern int RhCreateAndInject(
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InEXEPath,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InCommandLine,
				uint InProcessCreationFlags,
				uint InInjectionOptions,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x86,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string InLibraryPath_x64,
				IntPtr InPassThruBuffer,
				uint InPassThruSize,
				IntPtr OutProcessId //Pointer to a UINT (the PID of the new process)
				);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		static extern bool CreateProcess(
			string lpApplicationName,
			string lpCommandLine,
			//ref SECURITY_ATTRIBUTES lpProcessAttributes,
			//ref SECURITY_ATTRIBUTES lpThreadAttributes,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes, 
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			//IntPtr lpStartupInfo,
			[In] ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32.dll")]
		static extern uint ResumeThread(IntPtr hThread);

		[DllImport("user32.dll")]
		static extern uint WaitForInputIdle(IntPtr hProcess, uint dwMilliseconds);

		[DllImport("kernel32.dll")]
		static extern int SuspendThread(IntPtr hThread);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct STARTUPINFO
		{
			public Int32 cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SECURITY_ATTRIBUTES
		{
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public int bInheritHandle;
		}


		public static void Main(string[] args)
		{
			const int argsLengthHooksCPP = 20;
			const int argsLengthStartupHook = 10;

			//dllpath, exePath, base64CmdArgs, useAppdataSwitch, appdataSwitchIndex, dinputHookEnabled, findWindowHookEnabled, controllerIndex, findMutexHookEnabled, mutexTargets
			if (args.Length == argsLengthStartupHook)
			{
				bool Bfs(string s) => s.ToLower().Equals("true");

				int ntFwh = CreateAndInjectStartupHook(
					args[0], 
					args[1], 
					args[2], 
					Bfs(args[3]),
					int.Parse(args[4]), 
					Bfs(args[5]), 
					Bfs(args[6]), 
					byte.Parse(args[7]), 
					Bfs(args[8]),
					args[9]
					);

				Environment.Exit(ntFwh);
				return;
			}

			if (args.Length != argsLengthHooksCPP)
			{
				throw new ArgumentException($"Need exactly {argsLengthHooksCPP} arguments");
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
			bool HookGetKeyboardState = nextBool();
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
			writeBool(HookGetKeyboardState, x++);
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
		
		private static int CreateAndInjectStartupHook(string hookDllPath, string exePath, string base64CommandLineArgs, bool useAppdataSwitch, int appdataSwitchIndex, bool dinputHookEnabled, bool findWindowHookEnabled, byte controllerIndex, bool findMutexHookEnabled, string mutexTargets)
		{
			string cmdLineArgs = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64CommandLineArgs));
			IntPtr pOutPID = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));

			//"hl2_singleton_mutex&&&&&ValveHalfLifeLauncherMutex&&&&&Overkill Engine Game"
			var targetsBytes = Encoding.Unicode.GetBytes(mutexTargets);
			int targetsBytesLength = targetsBytes.Length;

			int size = 64 + targetsBytesLength;
			var data = new byte[size];
			data[0] = dinputHookEnabled ? (byte)1 : (byte)0;
			data[1] = findWindowHookEnabled ? (byte)1 : (byte)0;
			data[2] = controllerIndex;
			data[3] = useAppdataSwitch ? (byte) 0 : (byte) 1;
			data[4] = findMutexHookEnabled ? (byte) 1 : (byte) 0;

			data[5] = (byte)(targetsBytesLength >> 24);
			data[6] = (byte)(targetsBytesLength >> 16);
			data[7] = (byte)(targetsBytesLength >> 8);
			data[8] = (byte) targetsBytesLength;

			Array.Copy(targetsBytes, 0, data, 9, targetsBytesLength);
			
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(data, 0, ptr, size);
			
			if (!(appdataSwitchIndex > 0 && useAppdataSwitch))
			{
				int ret = Environment.Is64BitProcess ? 
					Injector64.RhCreateAndInject(exePath, cmdLineArgs, 0, 0, "", hookDllPath, ptr, (uint)size, pOutPID) : 
					Injector32.RhCreateAndInject(exePath, cmdLineArgs, 0, 0, hookDllPath, "", ptr, (uint)size, pOutPID);
				
				return ret;
			}

			var appdataIndex = "index" + appdataSwitchIndex;
			
			var sb = new StringBuilder();
			IDictionary envVars = Environment.GetEnvironmentVariables();
			var username = Environment.UserName;
			envVars["USERPROFILE"] =	$@"C:\Users\{username}\UniversalSplitScreen\{appdataIndex}";
			envVars["HOMEPATH"] =		$@"\Users\{username}\UniversalSplitScreen\{appdataIndex}";
			envVars["APPDATA"] =		$@"C:\Users\{username}\UniversalSplitScreen\{appdataIndex}\AppData\Roaming";
			envVars["LOCALAPPDATA"] =	$@"C:\Users\{username}\UniversalSplitScreen\{appdataIndex}\AppData\Local";

			//Some games will crash if the directories don't exist
			Directory.CreateDirectory(envVars["USERPROFILE"].ToString());
			Directory.CreateDirectory(Path.Combine(envVars["USERPROFILE"].ToString(), "Documents"));
			Directory.CreateDirectory(envVars["APPDATA"].ToString());
			Directory.CreateDirectory(envVars["LOCALAPPDATA"].ToString());
			
			foreach (object envVarKey in envVars.Keys)
			{
				if (envVarKey != null)
				{
					string key = envVarKey.ToString();
					string value = envVars[envVarKey].ToString();
					
					sb.Append(key);
					sb.Append("=");
					sb.Append(value);
					sb.Append("\0");
					
				}
			}

			sb.Append("\0");

			byte[] envBytes = Encoding.Unicode.GetBytes(sb.ToString());
			IntPtr envPtr = Marshal.AllocHGlobal(envBytes.Length);
			Marshal.Copy(envBytes, 0, envPtr, envBytes.Length);
			
			string directoryPath = Path.GetDirectoryName(exePath);

			STARTUPINFO startup = new STARTUPINFO();
			startup.cb = Marshal.SizeOf(startup);
			//startup.dwFlags = 0x1;//STARTF_USESHOWWINDOW
			//startup.wShowWindow = 1;//SW_SHOWNORMAL
			
			bool success = CreateProcess(exePath, cmdLineArgs, IntPtr.Zero, IntPtr.Zero, false,
				0x00000004 | 0x00000400, //Suspended | CREATE_UNICODE_ENVIRONMENT
				envPtr, directoryPath, ref startup, out PROCESS_INFORMATION processInformation);
			
			if (!success) return -1;
			
			int injectResult = Environment.Is64BitProcess ? 
				Injector64.RhInjectLibrary((uint)processInformation.dwProcessId, 0, 0, "", hookDllPath, ptr, (uint)size) : 
				Injector32.RhInjectLibrary((uint)processInformation.dwProcessId, 0, 0, hookDllPath, "", ptr, (uint)size);
			
			ResumeThread(processInformation.hThread);
			
			return injectResult;

		}
	}
}
