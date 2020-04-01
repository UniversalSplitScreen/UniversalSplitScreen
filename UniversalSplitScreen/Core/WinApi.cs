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

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

		public static class InjectorCPP64
		{
			[DllImport("InjectorCPP64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern uint Inject(
				int pid,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
				IntPtr hWnd,
				string ipcChannelName);
		}

		public static class InjectorCPP32
		{
			[DllImport("InjectorCPP32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern uint Inject(
				int pid,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath32,
				[MarshalAsAttribute(UnmanagedType.LPWStr)] string injectionDllPath64,
				IntPtr hWnd,
				string ipcChannelName);
		}

		[DllImport("SourceEngineUnlocker.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int SourceEngineUnlock(int pid, string targetName);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(int hwnd);


		public const int GWL_STYLE = -16;

		#region GetWindowLongPtr
		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		public static extern long GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		public static long GetWindowLongPtr(IntPtr hWnd, int nIndex)
		{
			return IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);
		}
		#endregion

		#region SetWindowLongPtr
		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : (IntPtr)SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32());
		}
		#endregion

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		
		public static bool RefreshWindow(IntPtr hWnd)
		{
			return SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
				0x0002 | 0x0001 | 0x0004 | 0x0020);//SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED
		}

		//[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		//public static extern bool DrawIcon(HandleRef hDC, int x, int y, HandleRef hIcon);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawIcon(IntPtr hDC, int x, int y, IntPtr hIcon);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref System.Drawing.Point lpPoint);

		[DllImport("EasyHook32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int RhIsX64Process(int InProcessId, out bool OutResult);
	}
}
