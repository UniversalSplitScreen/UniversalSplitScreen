using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;
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

		private class PointerForm : Form
		{
			IntPtr hicon;

			public int x;
			public int y;

			IntPtr hWnd;

			public PointerForm(IntPtr hWnd) : base()
			{
				this.hWnd = hWnd;

				BackColor = System.Drawing.Color.Green;
				TransparencyKey = BackColor;
				ShowInTaskbar = false;
				
				hicon = Cursors.Default.Handle;
			}

			protected override void OnPaintBackground(PaintEventArgs e)
			{
				base.OnPaintBackground(e);

				var graphics = System.Drawing.Graphics.FromHwnd(this.Handle);
				//var graphics = System.Drawing.Graphics.FromHwnd(hWnd);
				WinApi.DrawIcon(graphics.GetHdc(), 0, 0, hicon);

				/*Console.WriteLine("a");
				if (!Bounds.Contains(x, y))
				{
					//TODO: move location
					//TODO: set width/height to 100 (e.g.)
					Console.WriteLine("jmp");
					Location = new System.Drawing.Point(x - Width/2, y - Height/2);
				}

				var _g = System.Drawing.Graphics.FromHwnd(this.Handle);
				//var _g = System.Drawing.Graphics.FromHwnd(hWnd);
				Cursors.Default.Draw(_g, new System.Drawing.Rectangle(new System.Drawing.Point(x - Location.X, y - Location.Y), Cursors.Default.Size));
				this.Invalidate();

				//var graphics = System.Drawing.Graphics.FromHwnd(this.Handle);
				//var graphics = System.Drawing.Graphics.FromHwnd(hWnd);
				//WinApi.DrawIcon(graphics.GetHdc(), x - Location.X, y - Location.Y, hicon);*/
			}
		}

		private PointerForm pointerForm = null;

		public void CreateCursor()
		{
			pointerForm = new PointerForm(hWnd)
			{
				Width = 30,
				Height = 30,
				FormBorderStyle = FormBorderStyle.None,
				Text = "",
				StartPosition = FormStartPosition.Manual,
				Location = new System.Drawing.Point(0, 0),
				//Location = new System.Drawing.Point(Bounds.Left, Bounds.Right),
				TopMost = true
			};

			pointerForm.Show();
		}

		public void UpdateCursorPosition()
		{
			if (pointerForm != null)
			{
				var p = new System.Drawing.Point(MousePosition.x, MousePosition.y);
				WinApi.ClientToScreen(hWnd, ref p);
				

				//if (!pointerForm.Bounds.Contains(p.X, p.Y))
				//{
				//	Console.WriteLine("jmp2");
				//	pointerForm.Location = new System.Drawing.Point(p.X - pointerForm.Width / 2, p.Y - pointerForm.Height / 2);
				//}
				

				const int res = 5;

				int roundedX = res * (int)Math.Round((double)p.X / res);
				int roundedY = res * (int)Math.Round((double)p.Y / res);
				if (pointerForm.Location.X != roundedX || pointerForm.Location.Y != roundedY)
				{
					pointerForm.Location = new System.Drawing.Point(roundedX, roundedY);
				}
			}
		}

		public void KillCursor()
		{
			pointerForm?.Hide();
			pointerForm?.Dispose();
			pointerForm = null;
		}


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
