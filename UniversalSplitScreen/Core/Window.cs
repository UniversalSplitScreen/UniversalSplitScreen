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
		public readonly int pid;//Process ID
		
		public IntPtr MouseAttached { get; set; } = new IntPtr(0);
		public IntVector2 MousePosition { get; } = new IntVector2();//This is a reference type
		public (bool l, bool m, bool r, bool x1, bool x2) MouseState { get; set; } = (false, false, false, false, false);
		public byte WASD_State { get; set; } = 0;

		public IntPtr KeyboardAttached { get; set; } = new IntPtr(0);
		public readonly Dictionary<ushort, bool> keysDown = new Dictionary<ushort, bool>();//TODO: make this with 8 ints? (bool takes 1 byte, so 8x more memory than needed)

		public int ControllerIndex { get; set; } = 0;//0 = none, 1234 = 1234

		public WindowManagement.RECT Bounds { get; private set; }
		public int Width => Bounds.Right - Bounds.Left;
		public int Height => Bounds.Bottom - Bounds.Top;

		public NamedPipe HooksCPPNamedPipe { get; set; }

		public Window(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			WinApi.GetWindowThreadProcessId(hWnd, out this.pid);
			UpdateBounds();
			//Logger.WriteLine($"Bounds for hWnd={hWnd}: Left={Bounds.Left}, Right={Bounds.Right}, Top={Bounds.Top}, Bottom={Bounds.Bottom}, WIDTH={Width}, HEIGHT={Height}");
		}

		public void UpdateBounds()
		{
			WindowManagement.WinApi.GetWindowRect(hWnd, out var bounds);
			Bounds = bounds;
		}
	}
}
