using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			public int screenX;
			public int screenY;

			public bool visible = true;

			private int oldScreenX;
			private int oldScreenY;

			private int oldLocationX;
			private int oldLocationY;

			IntPtr hWnd;

			System.Reflection.MethodInfo paintBkgMethod;

			public PointerForm(IntPtr hWnd) : base()
			{
				this.hWnd = hWnd;

				BackColor = System.Drawing.Color.Green;
				TransparencyKey = BackColor;
				ShowInTaskbar = false;
				
				hicon = Cursors.Default.Handle;

				//g = System.Drawing.Graphics.FromHwnd(hWnd);
				//g = System.Drawing.Graphics.FromHwnd(this.Handle);
				//h = g.GetHdc();
				//h = System.Drawing.Graphics.FromHwnd(this.Handle).GetHdc();

				paintBkgMethod = typeof(Control).GetMethod("PaintBackground", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[] { typeof(PaintEventArgs), typeof(System.Drawing.Rectangle) }, null);
			}
			System.Drawing.Graphics g = null;
			IntPtr h;
			bool hasDrawn = false;

			const int cursorWidthHeight = 30;

			protected override void OnPaintBackground(PaintEventArgs e)
			{
				/*base.OnPaintBackground(e);
				
				var g = System.Drawing.Graphics.FromHwnd(this.Handle);
				//Cursors.Default.Draw(g, new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), Cursors.Default.Size));
				WinApi.DrawIcon(g.GetHdc(), 0, 0, hicon);*/

				//base.OnPaintBackground(e);

				

				if (!hasDrawn)
				{ 
					hasDrawn = true;
					base.OnPaintBackground(e);
					return;
				}

				/*if (oldLocationX != Location.X || oldLocationY != Location.Y)
				{
					base.OnPaintBackground(e);
					paintBkgMethod.Invoke(this, new object[]{e,
						new System.Drawing.Rectangle(oldScreenX - oldLocationX, oldScreenY - oldLocationY, cursorWidthHeight, cursorWidthHeight) });

					oldLocationX = Location.X;
					oldLocationY = Location.Y;
				}
				else*/
				{
					paintBkgMethod.Invoke(this, new object[]{e,
						new System.Drawing.Rectangle(oldScreenX - Location.X, oldScreenY - Location.Y, cursorWidthHeight, cursorWidthHeight) });
				}

				//var graphics = System.Drawing.Graphics.FromHwnd(this.Handle);
				//var graphics = System.Drawing.Graphics.FromHwnd(hWnd);

				//WinApi.DrawIcon(h, 0, 0, hicon);

				//var g = System.Drawing.Graphics.FromHwnd(this.Handle);
				if (g == null)
				{
					g = System.Drawing.Graphics.FromHwnd(this.Handle);
					h = g.GetHdc();
				}
				if (visible)
					WinApi.DrawIcon(h, screenX - Location.X, screenY - Location.Y, hicon);


				oldScreenX = screenX;
				oldScreenY = screenY;

				//Greatly reduces cpu usage, doesn't lock any input. Insignificant delay. Mouse cursor is only used in menus, not first person.
				//System.Threading.Thread.Sleep(5);

				System.Threading.Thread.Sleep(1);

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

			public void InvalidateMouse()
			{
				Invalidate(new System.Drawing.Rectangle(oldScreenX - Location.X, oldScreenY - Location.Y, cursorWidthHeight, cursorWidthHeight));
			}

			public void RepaintAll()
			{
				base.OnPaintBackground(new PaintEventArgs(System.Drawing.Graphics.FromHwnd(this.Handle), Bounds));
			}
		}

		private PointerForm pointerForm = null;
		public bool CursorVisibility { get => pointerForm.visible; set { if (pointerForm != null) pointerForm.visible = value; } }


		public void CreateCursor()
		{
			pointerForm = new PointerForm(hWnd)
			{
				Width = 2000,
				Height = 2000,
				FormBorderStyle = FormBorderStyle.None,
				Text = "",
				StartPosition = FormStartPosition.Manual,
				Location = new System.Drawing.Point(0, 0),
				//Location = new System.Drawing.Point(Bounds.Left + 7, Bounds.Top + 31),
				TopMost = true
			};

			pointerForm.Show();
		}

		bool hasRepaintedSinceLastInvisible = false;

		public void UpdateCursorPosition()
		{
			if (pointerForm != null)
			{
				

				if (pointerForm.visible)
				{
					hasRepaintedSinceLastInvisible = false;
					var p = new System.Drawing.Point(MousePosition.x, MousePosition.y);
					WinApi.ClientToScreen(hWnd, ref p);
					//pointerForm.Location = p;

					pointerForm.screenX = p.X;
					pointerForm.screenY = p.Y;



					const int padding = 35;
					if (p.X <= pointerForm.Location.X + padding || p.Y <= pointerForm.Location.Y + padding ||
						p.X >= pointerForm.Location.X + pointerForm.Width - padding || p.Y >= pointerForm.Location.Y + pointerForm.Height - padding)
					{
						pointerForm.RepaintAll();
						pointerForm.Location = new System.Drawing.Point(p.X - pointerForm.Width / 2, p.Y - pointerForm.Height / 2);

					}

					pointerForm.InvalidateMouse();
				}
				else if (!hasRepaintedSinceLastInvisible)
				{
					Logger.WriteLine("REPAINT");
					pointerForm.RepaintAll();
					hasRepaintedSinceLastInvisible = true;
				}

				//if (!pointerForm.Bounds.Contains(p.X, p.Y))
				//{
				//	Console.WriteLine("jmp2");
				//	pointerForm.Location = new System.Drawing.Point(p.X - pointerForm.Width / 2, p.Y - pointerForm.Height / 2);
				//}


				/*int oldX = pointerForm.x;
				int oldY = pointerForm.y;
				pointerForm.x = MousePosition.x;
				pointerForm.y = MousePosition.y;
				pointerForm.InvalidateMouse(oldX, oldY);*/



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
