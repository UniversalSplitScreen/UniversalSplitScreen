using System;

namespace UniversalSplitScreen.SendInput
{
	[Flags]
	enum KeyboardInputNotifications : ushort
	{
		WM_KEYDOWN = 0x0100,
		WM_KEYUP = 0x0101
	}
}
