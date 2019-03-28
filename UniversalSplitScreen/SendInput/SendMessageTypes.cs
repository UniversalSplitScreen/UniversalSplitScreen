using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.SendInput
{
	[Flags]
	public enum SendMessageTypes : ushort
	{
		WM_SETFOCUS = 0x0007,
		WM_ACTIVATE = 0x0006,
		WM_CAPTURECHANGED = 0x0215,
		WM_MOUSEACTIVATE = 0x0021,
		WM_KEYDOWN = 0x0100,
		WM_KEYUP = 0x0101,
		WM_INPUT = 0x00FF
	}
}
