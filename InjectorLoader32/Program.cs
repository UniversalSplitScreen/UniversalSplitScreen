using System;
using System.Runtime.InteropServices;

namespace InjectorLoader32
{
	static class Program
	{
		[DllImport("InjectorCPP32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern uint Inject(
				int pid,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
				IntPtr hWnd,
				string ipcChannelName);
		
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

			//InjectorCPP32 function
			uint nt = Inject(pid, injectionDllPath, "", hWnd, ipcChannelName);

			//Set exit code
			Environment.Exit((int)nt);
		}
	}
}
