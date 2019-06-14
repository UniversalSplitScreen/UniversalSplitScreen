using System;

namespace UniversalSplitScreen.SendInput
{
	[Flags]
	enum WM_MOUSEMOVE_wParam : ushort
	{
		MK_CONTROL = 0x0008,
		MK_LBUTTON = 0x0001,
		MK_MBUTTON = 0x0010,
		MK_RBUTTON = 0x0002,
		MK_SHIFT = 0x0004,
		MK_XBUTTON1 = 0x0020,
		MK_XBUTTON2 = 0x0040
	}
}
