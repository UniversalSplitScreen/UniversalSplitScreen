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
				IntPtr hmod);

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

		public static void Main(string[] args)
		{
			if (args.Length != 4)
			{
				throw new ArgumentException("Need exactly 4 arguments");
			}

			//Arguments
			int.TryParse(args[0], out int pid);

			string injectionDllPath = args[1];

			int.TryParse(args[2], out int _hWnd);
			IntPtr hWnd = (IntPtr)_hWnd;

			string ipcChannelName = args[3];

			IntPtr hmod = LoadLibrary(injectionDllPath);
			Console.WriteLine($"InjectorCPP hMod = {hmod}");

			//InjectorCPP64 function
			uint nt = Inject(pid, "", injectionDllPath, hWnd, ipcChannelName, hmod);

			//Set exit code
			Environment.Exit((int)nt);
		}
	}
}
