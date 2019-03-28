using System;
using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTHEADER
	{
		public uint dwType;
		public uint dwSize;
		public IntPtr hDevice;
		public IntPtr wParam;
	}
}
