using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UniversalSplitScreen.Core
{
	static class WinApi
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
		public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);
		
		public static string GetWindowText(IntPtr hWnd)
		{
			int length = GetWindowTextLength(hWnd) + 1;
			StringBuilder Buff = new StringBuilder(length);

			if (GetWindowText(hWnd, Buff, length) > 0)
			{
				return Buff.ToString();
			}
			return null;
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool IsWindow(IntPtr hWnd);


		public delegate int EnumWindowsProc(IntPtr hwnd, int lParam);
		[DllImport("user32.Dll")]
		public static extern int EnumWindows(EnumWindowsProc x, int y);
	}
}
