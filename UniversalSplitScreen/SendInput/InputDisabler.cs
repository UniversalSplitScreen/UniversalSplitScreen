using AutoHotkey.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UniversalSplitScreen.Core;

namespace UniversalSplitScreen.SendInput
{
	class InputDisabler
	{
		//https://github.com/amazing-andrew/AutoHotkey.Interop

		private static AutoHotkeyEngine ahk;
		public static bool IsAutoHotKeyNull => ahk == null;
		private static bool IsInitialised = false;
		private static Task initTask = null;
		
		[DllImport("user32.dll")]
		static extern int ShowCursor(bool bShow);

		[DllImport("user32.dll")]
		static extern IntPtr SetCursor(IntPtr handle);

		//TODO: move to start of program in separate thread so it doesn't cause delay when start is first clicked

		public static void Init()
		{
			initTask = Task.Run(() => 
			{
				try
				{
					Logger.WriteLine("Initialising InputDisabler");
					ahk = AutoHotkeyEngine.Instance;
					ahk.Suspend();
					
					//The star means it will disable even with modifier keys e.g. Shift
					
					ahk.ExecRaw("*MButton:: return");
					ahk.ExecRaw("*XButton1:: return");
					ahk.ExecRaw("*XButton2:: return");

					ahk.ExecRaw("*LWin:: return");
					ahk.ExecRaw("*Control:: return");
					ahk.ExecRaw("*Alt:: return");
					ahk.ExecRaw("*Shift:: return");//Important or shift will not function properly in game

					ahk.ExecRaw("*RButton:: return");
					ahk.ExecRaw("*LButton:: return");

					ahk.Suspend();

					IsInitialised = true;

					Logger.WriteLine("Initialised InputDisabler");
				}
				catch
				{
					Logger.WriteLine("Could not load InputDisabler");
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
			
			if (ahk != null) System.Windows.Forms.Cursor.Hide();//Only works if the form window in the top left corner (0,0)
			int i = 0;
			while (ShowCursor(false) >= 0 || i++ > 30);
			SetCursor(IntPtr.Zero);


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
