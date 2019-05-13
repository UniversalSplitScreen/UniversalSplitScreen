using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.RawInput
{
	//https://docs.microsoft.com/en-gb/windows/desktop/inputdev/keyboard-input-notifications
	public enum KeyboardMessages : uint
	{
		WM_KEYDOWN = 0x0100,
		WM_KEYUP = 0x0101
	}
}
