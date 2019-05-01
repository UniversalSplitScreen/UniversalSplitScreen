using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels.Ipc;
using UniversalSplitScreen.Piping;

namespace UniversalSplitScreen.Core
{
	public class Window
	{
		public readonly IntPtr hWnd;
		public IntPtr borderlands2_DIEmWin_hWnd = IntPtr.Zero;//WM_INPUT needs to be sent to this hWnd instead of the visible game hWnd or it is ignored.
		public readonly int pid;

		public GetRawInputDataHook.ServerInterface GetRawInputData_HookServer { get; set; } = null;
		public IpcServerChannel GetRawInputData_HookIPCServerChannel { get; set; } = null;

		public IntPtr MouseAttached { get; set; } = new IntPtr(0);
		public IntVector2 MousePosition { get; } = new IntVector2();
		public (bool l, bool m, bool r, bool x1, bool x2) MouseState { get; set; } = (false, false, false, false, false);

		public IntPtr KeyboardAttached { get; set; } = new IntPtr(0);
		public readonly Dictionary<ushort, bool> keysDown = new Dictionary<ushort, bool>();

		public WindowManagement.RECT Bounds { get; private set; }
		public int Width => Bounds.Right - Bounds.Left;
		public int Height => Bounds.Bottom - Bounds.Top;

		public NamedPipe HooksCPPNamedPipe { get; set; }

		public Window(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			WinApi.GetWindowThreadProcessId(hWnd, out this.pid);
			UpdateBounds();
			//Console.WriteLine($"Bounds for hWnd={hWnd}: Left={Bounds.Left}, Right={Bounds.Right}, Top={Bounds.Top}, Bottom={Bounds.Bottom}, WIDTH={Width}, HEIGHT={Height}");
		}

		public void UpdateBounds()
		{
			WindowManagement.WinApi.GetWindowRect(hWnd, out var bounds);
			Bounds = bounds;
		}
	}
}
