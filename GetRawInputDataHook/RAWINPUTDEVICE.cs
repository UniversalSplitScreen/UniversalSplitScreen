using System;

namespace GetRawInputDataHook
{
	//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagrawinputdevice
	public struct RAWINPUTDEVICE
	{
		internal ushort usUsagePage;
		internal ushort usUsage;
		internal uint dwFlags;
		internal IntPtr hwndTarget;

		public override string ToString()
		{
			return string.Format("{0}/{1}, flags: {2}, target: {3}", usUsagePage, usUsage, dwFlags, hwndTarget);
		}
	}
}
