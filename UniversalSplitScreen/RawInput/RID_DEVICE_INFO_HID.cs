using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagrid_device_info_hid
	[StructLayout(LayoutKind.Sequential)]
	public struct RID_DEVICE_INFO_HID
	{
		/// <summary>
		/// The vendor identifier for the HID.
		/// </summary>
		public uint dwVendorId;

		/// <summary>
		/// The product identifier for the HID.
		/// </summary>
		public uint dwProductId;

		/// <summary>
		/// The version number for the HID.
		/// </summary>
		public uint dwVersionNumber;

		/// <summary>
		/// The top-level collection Usage Page for the device.
		/// </summary>
		public ushort usUsagePage;

		/// <summary>
		/// The top-level collection Usage for the device.
		/// </summary>
		public ushort usUsage;
	}
}
