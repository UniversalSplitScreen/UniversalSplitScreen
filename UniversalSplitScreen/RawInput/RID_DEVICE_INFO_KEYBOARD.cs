using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagrid_device_info_keyboard
	[StructLayout(LayoutKind.Sequential)]
	public struct RID_DEVICE_INFO_KEYBOARD
	{
		/// <summary>
		/// The type of the keyboard.
		/// </summary>
		public uint dwType;

		/// <summary>
		/// The subtype of the keyboard.
		/// </summary>
		public uint dwSubType;

		/// <summary>
		/// The scan code mode.
		/// </summary>
		public uint dwKeyboardMode;

		/// <summary>
		/// The number of function keys on the keyboard.
		/// </summary>
		public uint dwNumberOfFunctionKeys;

		/// <summary>
		/// The number of LED indicators on the keyboard.
		/// </summary>
		public uint dwNumberOfIndicators;

		/// <summary>
		/// The total number of keys on the keyboard.
		/// </summary>
		public uint dwNumberOfKeysTotal;
	}
}
