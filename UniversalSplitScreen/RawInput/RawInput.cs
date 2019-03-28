using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUT
	{
		public RAWINPUTHEADER header;
		public RAWINPUT_DATA data;
	}
}
