using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	[StructLayout(LayoutKind.Explicit)]
	public struct RAWINPUT_DATA
	{
		[FieldOffset(0)]
		public RAWMOUSE mouse;

		[FieldOffset(0)]
		public RAWKEYBOARD keyboard;

		[FieldOffset(0)]
		public RAWHID hid;
	}
}
