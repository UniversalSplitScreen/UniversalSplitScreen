using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.RawInput
{
	//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagrid_device_info
	[StructLayout(LayoutKind.Explicit)]
	public struct RID_DEVICE_INFO
	{
		[FieldOffset(0)]
		public uint cbSize;

		///<summary>
		///The type of raw input data. This member can be one of the following values.
		///RIM_TYPEHID = 2 : Data comes from an HID that is not a keyboard or a mouse.
		///RIM_TYPEKEYBOARD = 1 : Data comes from a keyboard.
		///RIM_TYPEMOUSE = 0 : Data comes from a mouse.
		///</summary>
		[FieldOffset(4)]
		public uint dwType;
		
		[FieldOffset(8)]
		public RID_DEVICE_INFO_MOUSE mouse;
		
		[FieldOffset(8)]
		public RID_DEVICE_INFO_KEYBOARD keyboard;
		
		[FieldOffset(8)]
		public RID_DEVICE_INFO_HID hid;
	}
}
