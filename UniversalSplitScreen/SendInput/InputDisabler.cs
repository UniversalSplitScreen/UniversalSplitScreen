using AutoHotkey.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.SendInput
{
	class InputDisabler
	{
		//https://github.com/amazing-andrew/AutoHotkey.Interop

		private static AutoHotkeyEngine ahk;
		public static bool IsAutoHotKeyNull => ahk == null;
		private static bool IsInitialised = false;
		private static Task initTask = null;

		#region Windows API
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(int hwnd);

		[DllImport("user32.dll")]
		private static extern int GetDesktopWindow();

		[DllImport("user32.dll")]
		static extern int ShowCursor(bool bShow);
		#endregion

		//TODO: move to start of program in separate thread so it doesnt cause delay when start is first clicked

		public static void Init()
		{
			initTask = Task.Run(() => //TODO: this delays the UI from responding?
			{
				try
				{
					ahk = AutoHotkeyEngine.Instance;
					ahk.ExecRaw("*RButton:: return");//The star means it will disable even with modifier keys e.g. Shift
					ahk.ExecRaw("*LButton:: return");
					ahk.ExecRaw("*MButton:: return");
					ahk.ExecRaw("*XButton1:: return");
					ahk.ExecRaw("*XButton2:: return");

					ahk.ExecRaw("*LWin:: return");
					ahk.ExecRaw("*Control:: return");
					ahk.ExecRaw("*Alt:: return");
					ahk.ExecRaw("*Shift:: return");//Important or shift will not function properly in game
					
					//ahk.ExecRaw("*Space:: return");//Prevents space being detected in minecraft?

					/*foreach (char c in "wasd")//Prevents characters being detected in raw input
					{
						ahk.ExecRaw($"*{c}:: return");
					}*/

					ahk.Suspend();

					IsInitialised = true;

					Console.WriteLine("Initialised InputDisabler");
				}
				catch
				{
					Console.WriteLine("Could not load Mouse Disabler");
				}
			});
		}
		
		public static async void Lock()
		{
			if (!IsInitialised)
			{
				if (initTask == null)
					Init();
				
				await initTask;
			}

			ahk?.UnSuspend();
			SetForegroundWindow(GetDesktopWindow());//Loses focus of all windows, without minimizing
			if (ahk != null) System.Windows.Forms.Cursor.Hide();//Only works if the form window in the top left corner (0,0)

			SendInput.WinApi.BlockInput(true);
		}

		public static void Unlock()
		{
			if (ahk != null)
			{
				ahk.Suspend();
				System.Windows.Forms.Cursor.Show();
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();

				/* https://msdn.microsoft.com/en-us/library/windows/desktop/ms633539.aspx
					An application cannot force a window to the foreground while the user is working with another window. Instead, Windows flashes the taskbar button of the window to notify the user.
						^^^ (doesn't work) */
				//SetForegroundWindow((int));
			}

			SendInput.WinApi.BlockInput(false);
		}
	}
}
