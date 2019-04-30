using System;
using System.Runtime.InteropServices;

namespace UniversalSplitScreen.Piping
{
	class WinApi
	{
		[DllImport("kernel32.dll")]
		public static extern bool CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe, ref SECURITY_ATTRIBUTES lpPipeAttributes, uint nSize);

		[DllImport("kernel32.dll")]
		public static extern bool WriteFile(
			IntPtr hFile,
			byte[] lpBuffer, 
			uint nNumberOfBytesToWrite,
			out uint lpNumberOfBytesWritten,
			[In] ref System.Threading.NativeOverlapped lpOverlapped);
	}
}
