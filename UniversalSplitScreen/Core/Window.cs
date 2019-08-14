using System;
using System.Collections;
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
		public readonly BitArray keysDown = new BitArray(0xFF);

		public int ControllerIndex { get; set; } = 0;//0 = none, 1234 = 1234

		public WindowManagement.RECT Bounds { get; private set; }
		public int Width => Bounds.Right - Bounds.Left;
		public int Height => Bounds.Bottom - Bounds.Top;

		public NamedPipe HooksCPPNamedPipe { get; set; }

		#region Mouse Cursor
		/* How drawing the fake mouse cursor works:
		 * A transparent window (PointerForm) is created over the game window.
		 * When the mouse is moved, UpdateCursorPosition will tell the window to paint over the old cursor (wiping it) and draw the new one.
		 * If the mouse moves out of bounds of PointerForm, the centre of the window is moved to the mouse position.
		 * (Depends if Hook mouse visibility is enabled) In HooksCPP, SetCursor(NULL or not NULL) and ShowCursor(TRUE/FALSE) is monitored to show/hide cursor.
		 * When this is detected, it sends a message via a named pipe to set CursorVisibility, which which will wipe/draw the cursor.
		 * 
		 */

		private class PointerForm : Form
		{
			IntPtr hicon;

			public int screenX;
			public int screenY;

			public bool visible = true;

			private int oldScreenX;
			private int oldScreenY;

			IntPtr hWnd;

			System.Reflection.MethodInfo paintBkgMethod;

			System.Drawing.Graphics g = null;
			IntPtr h;
			bool hasDrawn = false;//We need to paint the entire window once at the start.

			const int windowWidth = 1300;//Minimum width
			const int windowHeight = 800;

			public PointerForm(IntPtr hWnd, int gameWindowWidth, int gameWindowHeight, int gameWindowX, int gameWindowY) : base()
			{
				this.hWnd = hWnd;

				Width = Math.Max(windowWidth, gameWindowWidth + 100);
				Height = Math.Max(windowHeight, gameWindowHeight + 100);
				Logger.WriteLine($"Cursor window width,height = {Width},{Height}");
				FormBorderStyle = FormBorderStyle.None;
				Text = "";
				StartPosition = FormStartPosition.Manual;
				Location = new System.Drawing.Point(gameWindowX + gameWindowWidth/2, gameWindowY + gameWindowHeight/2);
				TopMost = true;
				BackColor = System.Drawing.Color.Green;
				TransparencyKey = BackColor;
				ShowInTaskbar = false;

				hicon = Cursors.Arrow.Handle;

				paintBkgMethod = typeof(Control).GetMethod("PaintBackground", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[] { typeof(PaintEventArgs), typeof(System.Drawing.Rectangle) }, null);
			}
			

			const int cursorWidthHeight = 19;

			protected override void OnPaintBackground(PaintEventArgs e)
			{			
				if (!hasDrawn)
				{ 
					hasDrawn = true;
					base.OnPaintBackground(e);
					return;
				}
				
				//This only wipes a small area, which reduces CPU usage.
				paintBkgMethod.Invoke(this, new object[]{e,
					new System.Drawing.Rectangle(oldScreenX - Location.X, oldScreenY - Location.Y, cursorWidthHeight, cursorWidthHeight) });
				
				if (g == null)
				{
					g = System.Drawing.Graphics.FromHwnd(this.Handle);
					h = g.GetHdc();
				}

				if (visible)
					WinApi.DrawIcon(h, screenX - Location.X, screenY - Location.Y, hicon);//Coordinates are relative to Location of PointerForm
				
				oldScreenX = screenX;
				oldScreenY = screenY;

				//Greatly reduces CPU usage, doesn't lock any input. Insignificant delay. Mouse cursor is only used in menus, not first person.
				System.Threading.Thread.Sleep(1);
			}

			public void InvalidateMouse()
			{
				//Causes this region to be re-drawn
				//Invalidate(new System.Drawing.Rectangle(oldScreenX - Location.X, oldScreenY - Location.Y, cursorWidthHeight, cursorWidthHeight)); (Seems to work inconsistently)
				Invalidate();
			}

			//Wipes the entire window
			public void RepaintAll()
			{
				try
				{
					base.OnPaintBackground(new PaintEventArgs(System.Drawing.Graphics.FromHwnd(this.Handle), Bounds));
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error in RepaintAll: {e}");
				}
			}
		}

		private PointerForm pointerForm = null;
		public bool CursorVisibility {
			get => pointerForm.visible;
			set {
				if (pointerForm != null)
				{
					pointerForm.visible = value;
					//UpdateCursorPosition();
				}
			}
		}
		
		public void CreateCursor()
		{
			pointerForm = new PointerForm(hWnd, Width, Height, Bounds.Left, Bounds.Top);
			pointerForm.Show();
		}

		bool hasRepaintedSinceLastInvisible = false;

		public void UpdateCursorPosition()
		{
			if (pointerForm != null)//If Draw Mouse is selected
			{
				if (pointerForm.visible)
				{
					hasRepaintedSinceLastInvisible = false;
					var p = new System.Drawing.Point(MousePosition.x, MousePosition.y);
					WinApi.ClientToScreen(hWnd, ref p);

					pointerForm.screenX = p.X;
					pointerForm.screenY = p.Y;
									   
					const int padding = 32;
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
					//If the cursor should be invisible, make sure the last cursor has been wiped.
					pointerForm.RepaintAll();
					hasRepaintedSinceLastInvisible = true;
				}
			}
		}

		public void KillCursor()
		{
			pointerForm?.Hide();
			pointerForm?.Dispose();
			pointerForm = null;
		}
		#endregion

		public Window(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			WinApi.GetWindowThreadProcessId(hWnd, out this.pid);
			UpdateBounds();

			//Logger.WriteLine($"Bounds for hWnd={hWnd}: Left={Bounds.Left}, Right={Bounds.Right}, Top={Bounds.Top}, Bottom={Bounds.Bottom}, WIDTH={Width}, HEIGHT={Height}");
		}

		public void UpdateBounds()
		{
			WindowManagement.WinApi.GetClientRect(hWnd, out var bounds);
			Bounds = bounds;
		}
	}
}
