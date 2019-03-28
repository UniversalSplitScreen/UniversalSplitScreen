using System;
using System.Runtime.InteropServices;

namespace UniversalSplitScreen.WindowManagement
{
	public static class WinApi
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
	}
}
