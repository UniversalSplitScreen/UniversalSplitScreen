using System.Runtime.InteropServices;

namespace GetRawInputDataHook
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUT
	{
		public RAWINPUTHEADER header;
		public RAWINPUT_DATA data;
	}
}
