using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.Piping
{
	class AnonymousPipe
	{
		IntPtr hReadPipe;
		IntPtr hWritePipe;

		public AnonymousPipe()
		{
			SECURITY_ATTRIBUTES s = new SECURITY_ATTRIBUTES()
			{
				nLength = Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES)),
				lpSecurityDescriptor = IntPtr.Zero,
				bInheritHandle = 0
			};

			WinApi.CreatePipe(out hReadPipe, out hWritePipe, ref s, 9);

			
		}

		public IntPtr GetReadHandle()
		{
			return hReadPipe;
		}

		public void TestWrite()
		{
			byte[] bytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

			System.Threading.NativeOverlapped x = new System.Threading.NativeOverlapped();

			bool success = WinApi.WriteFile(hWritePipe, bytes, 9, out uint bytesWritten, ref x);

			Console.WriteLine($"Bytes written = {bytesWritten}, success = {success}");
		}
	}
}
