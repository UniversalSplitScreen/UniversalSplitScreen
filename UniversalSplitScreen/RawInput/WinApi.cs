using System;
using System.Runtime.InteropServices;

namespace UniversalSplitScreen.RawInput
{
	static class WinApi
	{
		//https://docs.microsoft.com/en-gb/windows/desktop/inputdev/wm-input
		public const int WM_INPUT = 0x00FF;

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-registerrawinputdevices
		[DllImport("User32.dll")]
		public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getrawinputdata
		[DllImport("User32.dll")]
		public static extern int GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, [Out] IntPtr pData, ref uint pcbSize, int cbSizeHeader);
		[DllImport("User32.dll")]
		public static extern int GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);
		

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getrawinputdevicelist
		[DllImport("User32.dll")]
		public static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint puiNumDevices, uint cbSize);


		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getrawinputdeviceinfoa
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hDevice"></param>
		/// <param name="uiCommand">
		///RIDI_DEVICENAME = 0x20000007 : pData points to a string that contains the device name. For this uiCommand only, the value in pcbSize is the character count (not the byte count).
		///RIDI_DEVICEINFO = 0x2000000b : pData points to an RID_DEVICE_INFO structure.
		///RIDI_PREPARSEDDATA = 0x20000005 : pData points to the previously parsed data. 
		///</param>
		/// <param name="pData"></param>
		/// <param name="pcbSize"></param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-findwindowa
		[DllImport("User32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
	}
}
