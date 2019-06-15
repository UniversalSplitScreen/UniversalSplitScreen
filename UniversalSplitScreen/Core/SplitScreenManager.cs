using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.Piping;
using UniversalSplitScreen.RawInput;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen.Core
{
	class SplitScreenManager
	{
		public bool IsRunningInSplitScreen { get; private set; } = false;

		Dictionary<Task, CancellationTokenSource> setFocusTasks = new Dictionary<Task, CancellationTokenSource>();
		Dictionary<Task, CancellationTokenSource> drawMouseTasks = new Dictionary<Task, CancellationTokenSource>();

		//Sometimes the game can be focused, which can break input. Fix: every X seconds, focus the desktop
		(Task, CancellationTokenSource) autoUnfocusTask;

		IntPtr active_hWnd = IntPtr.Zero;//Excludes self
		IntPtr desktop_hWnd = IntPtr.Zero;

		WinApi.WinEventDelegate EVENT_SYSTEM_FOREGROUND_delegate = null;
		const ushort EVENT_SYSTEM_FOREGROUND = 0x0003;//https://docs.microsoft.com/en-us/windows/desktop/WinAuto/event-constants
		const ushort WINEVENT_OUTOFCONTEXT = 0x0000;

		public readonly Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();
		private readonly Dictionary<IntPtr, Window[]> deviceToWindows = new Dictionary<IntPtr, Window[]>();

		public Window[] GetWindowsForDevice(IntPtr hDevice)
		{
			return deviceToWindows.TryGetValue(hDevice, out Window[] windows) ? windows : new Window[0];
		}

		#region Public methods

		/// <summary>
		/// Initializes the EVENT_SYSTEM_FOREGROUND hook (for monitoring the foreground window)
		/// </summary>
		public void Init()
		{
			Logger.WriteLine("Registering EVENT_SYSTEM_FOREGROUND hook");
			EVENT_SYSTEM_FOREGROUND_delegate = new WinApi.WinEventDelegate(EVENT_SYSTEM_FOREGROUND_Proc);
			IntPtr m_hhook = WinApi.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, EVENT_SYSTEM_FOREGROUND_delegate, 0, 0, WINEVENT_OUTOFCONTEXT);
		}

		/// <summary>
		/// deviceToWindows is a Dictionary with device handle as the key and the attached windows as the value.
		/// </summary>
		void InitDeviceToWindows()
		{
			deviceToWindows.Clear();
			foreach (var pair in windows)
			{
				Window window = pair.Value;
				
				if (!deviceToWindows.ContainsKey(window.MouseAttached))
					deviceToWindows[window.MouseAttached] = windows.Values.Where(x => x.MouseAttached == window.MouseAttached).ToArray();

				if (!deviceToWindows.ContainsKey(window.KeyboardAttached))
					deviceToWindows[window.KeyboardAttached] = windows.Values.Where(x => x.KeyboardAttached == window.KeyboardAttached).ToArray();
			}
		}

		/// <summary>
		/// Starts running split screen
		/// </summary>
		public void ActivateSplitScreen()
		{
			var options = Options.CurrentOptions;

			//Check if windows still exist
			{
				var toRemove = new List<IntPtr>();
				for (int i = 0; i < windows.Count; i++)
				{
					IntPtr hWnd = windows.ElementAt(i).Key;
					if (!WinApi.IsWindow(hWnd))
						toRemove.Add(hWnd);
				}

				foreach (var hWnd in toRemove)
					windows.Remove(hWnd);
			}

			foreach (var pair in windows)
			{
				IntPtr hWnd = pair.Key;
				Window window = pair.Value;

				Logger.WriteLine($"hWnd={hWnd}, mouse={window.MouseAttached}, kb={window.KeyboardAttached}");
				
				//Borderlands 2 requires WM_INPUT to be sent to a window named DIEmWin, not the main hWnd.
				foreach (ProcessThread thread in Process.GetProcessById(window.pid).Threads)
				{

					int WindowEnum(IntPtr _hWnd, int lParam)
					{
						int threadID = WinApi.GetWindowThreadProcessId(_hWnd, out int pid);
						if (threadID == lParam)
						{
							string windowText = WinApi.GetWindowText(_hWnd);
							Logger.WriteLine($" - thread id=0x{threadID:x}, _hWnd=0x{_hWnd:x}, window text={windowText}");

							if (windowText != null && windowText.ToLower().Contains("DIEmWin".ToLower()))//TODO: make configurable
							{
								window.borderlands2_DIEmWin_hWnd = _hWnd;
							}
						}

						return 1;
					}

					WinApi.EnumWindows(WindowEnum, thread.Id);
				}

				//WM_ACTIVATE/WM_SETFOCUS tasks
				if (options.SendWM_ACTIVATE || options.SendWM_SETFOCUS)
				{
					CancellationTokenSource c = new CancellationTokenSource();
					Task task = new Task(() => SetFocus(pair.Key, c.Token), c.Token);
					task.Start();
					setFocusTasks.Add(task, c);
				}

				//Draw mouse tasks
				if (options.DrawMouse)
				{
					CancellationTokenSource c = new CancellationTokenSource();
					Task task = new Task(() => DrawMouse(hWnd, c.Token), c.Token);
					task.Start();
					drawMouseTasks.Add(task, c);
				}
				
				//EasyHook
				if (options.Hook_FilterRawInput || 
					options.Hook_FilterWindowsMouseInput || 
					options.Hook_GetForegroundWindow || 
					options.Hook_GetCursorPos || 
					options.Hook_GetKeyState || 
					options.Hook_GetAsyncKeyState ||
					options.Hook_SetCursorPos ||
					options.Hook_XInput ||
					options.Hook_UseLegacyInput)
				{


					//bool needPipe = options.Hook_GetCursorPos || options.Hook_GetAsyncKeyState || options.Hook_GetKeyState;
					bool needPipe = true;
					NamedPipe pipe = needPipe ? new NamedPipe(hWnd) : null;
					window.HooksCPPNamedPipe = pipe;
						
					string hooksLibrary32 = Path.Combine(Path.GetDirectoryName(
						System.Reflection.Assembly.GetExecutingAssembly().Location),
						"HooksCPP32.dll");

					string hooksLibrary64 = Path.Combine(Path.GetDirectoryName(
						System.Reflection.Assembly.GetExecutingAssembly().Location),
						"HooksCPP64.dll");


					bool is64 = EasyHook.RemoteHooking.IsX64Process(window.pid);
						
					Process proc = new Process();
					proc.StartInfo.FileName = is64 ? "InjectorLoaderx64.exe" : "InjectorLoaderx86.exe";

					//Arguments
					{
						object[] args = new object[]
						{
							window.pid,
							$"\"{(is64 ? hooksLibrary64 : hooksLibrary32)}\"",
							window.hWnd,
							needPipe ? pipe.pipeName : "USS_NO_PIPE_NEEDED",
							window.ControllerIndex,
							(int)window.MouseAttached,
							options.Hook_UseLegacyInput,
							options.Hook_GetCursorPos,
							options.Hook_GetForegroundWindow,
							options.Hook_GetAsyncKeyState,
							options.Hook_GetKeyState,
							options.Hook_FilterWindowsMouseInput,
							options.Hook_FilterRawInput,
							options.Hook_SetCursorPos,
							options.Hook_XInput
						};

						StringBuilder sb = new StringBuilder();
						foreach (var arg in args)
						{
							sb.Append(" " + arg);
						}

						proc.StartInfo.Arguments = sb.ToString();
					}

					proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					proc.Start();
					proc.WaitForExit();

					uint exitCode = (uint)proc.ExitCode;
					Logger.WriteLine($"InjectorCPP.Inject result = 0x{exitCode:x}. is64={is64}, needPipe={needPipe}");
					if (exitCode != 0 )
					{
						MessageBox.Show($"Error injecting hooks into pid={window.pid}, Error = 0x{exitCode:x}", "Error");
						DeactivateSplitScreen();
						return;
					}
				}
			}

			//Auto unfocus task
			{
				CancellationTokenSource c = new CancellationTokenSource();
				Task task = new Task(() => AutoUnfocusTask(c.Token), c.Token);
				task.Start();
				autoUnfocusTask = (task, c);
			}
			
			IsRunningInSplitScreen = true;
			InputDisabler.Lock();
			Intercept.InterceptEnabled = true;
			deviceToWindows.Clear();
			InitDeviceToWindows();
			Cursor.Position = new System.Drawing.Point(0, 0);
			WinApi.SetForegroundWindow((int)WinApi.GetDesktopWindow());//Loses focus of all windows, without minimizing
			Program.Form.WindowState = FormWindowState.Minimized;

			Program.Form.OnSplitScreenStart();
		}
		
		public void DeactivateSplitScreen()
		{
			IsRunningInSplitScreen = false;
			InputDisabler.Unlock();
			Intercept.InterceptEnabled = false;

			foreach (var thread in setFocusTasks)
				thread.Value.Cancel();

			foreach (var thread in drawMouseTasks)
				thread.Value.Cancel();

			autoUnfocusTask.Item2?.Cancel();

			foreach (var window in windows.Values.ToArray())
			{
				window.HooksCPPNamedPipe?.Close();
			}

			setFocusTasks.Clear();
			drawMouseTasks.Clear();

			Program.Form.OnSplitScreenEnd();
			Program.Form.WindowState = FormWindowState.Normal;
		}

		public void UnlockSourceEngine()
		{
			string title = "";
			string msg = "";
			try
			{
				WinApi.GetWindowThreadProcessId(active_hWnd, out int pid);
				int seu = WinApi.SourceEngineUnlock(pid);
				Logger.WriteLine($"Source engine unlock return = {seu}");

				msg = seu == 1 ?	"Successfully unlocked game. Launch another instance from the exe file." : 
					(seu == 0 ?		"Couldn't find the source engine mutex" : 
									$"Error while finding mutex: {seu}");

				title = seu == 1 ? "Success" : "Error";
			}
			catch(Exception e)
			{
				msg = $"Exception while finding mutex: {e.GetType()}";
				title = "Error";
			}

			MessageBox.Show(msg, title);
		}

		public void AllowWindowResize()
		{
			int x = (int)WinApi.GetWindowLongPtr32(active_hWnd, WinApi.GWL_STYLE);
			x |= 0x00040000;
			//int x = 0 | 0x00020000 | 0x00080000 | 0x00010000 | 0x00C00000 | 0x10000000 | 0x00040000;
			WinApi.SetWindowLong32(active_hWnd, WinApi.GWL_STYLE, x);
			WinApi.RefreshWindow(active_hWnd);
		}

		public void ToggleWindowBorders()
		{
			const int flip = 0x00C00000 | 0x00080000;//WS_BORDER | WS_SYSMENU

			int x = (int)WinApi.GetWindowLongPtr32(active_hWnd, WinApi.GWL_STYLE);
			x ^= flip;
			WinApi.SetWindowLong32(active_hWnd, WinApi.GWL_STYLE, x);
			WinApi.RefreshWindow(active_hWnd);
		}

		#region Set handles
		public void SetMouseHandle(IntPtr mouse)
		{
			if (!windows.ContainsKey(active_hWnd))
				windows[active_hWnd] = new Window(active_hWnd);

			Window window = windows[active_hWnd];
			window.MouseAttached = mouse;

			Program.Form.MouseHandleText = mouse.ToString();

			if ((int)mouse == 0 && (int)window.KeyboardAttached == 0 && window.ControllerIndex == 0)
				windows.Remove(active_hWnd);
		}

		public void SetKeyboardHandle(IntPtr keyboard)
		{
			if (!windows.ContainsKey(active_hWnd)) windows[active_hWnd] = new Window(active_hWnd);

			Window window = windows[active_hWnd];
			window.KeyboardAttached = keyboard;

			Program.Form.KeyboardHandleText = keyboard.ToString();

			if ((int)keyboard == 0 && (int)window.MouseAttached == 0 && window.ControllerIndex == 0)
				windows.Remove(active_hWnd);
		}

		public void SetControllerIndex(int index)
		{
			if (!windows.ContainsKey(active_hWnd)) windows[active_hWnd] = new Window(active_hWnd);

			Window window = windows[active_hWnd];
			window.ControllerIndex = index;

			if ((int)window.KeyboardAttached == 0 && (int)window.MouseAttached == 0 && window.ControllerIndex == 0)
				windows.Remove(active_hWnd);
		}

		public void ResetAllHandles()
		{
			windows.Clear();
			Program.Form.MouseHandleText = "0";
			Program.Form.KeyboardHandleText = "0";
			Program.Form.ControllerSelectedIndex = 0;
		}
		#endregion

		#endregion

		#region Threaded tasks
		/// <summary>
		/// Periodically sends WM_ACTIVATE and WM_SETFOCUS to trick the target game into thinking it is the foreground.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="token"></param>
		private void SetFocus(IntPtr hWnd, CancellationToken token)
		{
			while(true)
			{
				Thread.Sleep(1000);//TODO: configurable this

				if (Options.CurrentOptions.SendWM_ACTIVATE)
					SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_ACTIVATE, (IntPtr)2, (IntPtr)null);//2 or 1?

				if (Options.CurrentOptions.SendWM_SETFOCUS)
					SendInput.WinApi.PostMessageA(hWnd, (uint)SendMessageTypes.WM_SETFOCUS, (IntPtr)null, (IntPtr)null);
				
				if (token.IsCancellationRequested)
					return;

			}
		}

		/// <summary>
		/// Periodically draws a mouse cursor over the game. 
		/// The cursor is wiped every time the game updates a frame. 
		/// The cursor will be flickery unless vsync is enabled in the game.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="token"></param>
		private void DrawMouse(IntPtr hWnd, CancellationToken token)
		{
			var g = System.Drawing.Graphics.FromHwnd(hWnd);

			while (true)
			{
				Thread.Sleep(Options.CurrentOptions.DrawMouseEveryXMilliseconds);

				if (windows.TryGetValue(hWnd, out Window window))
				{
					var mouseVec = window.MousePosition;
					var x = mouseVec.x;
					var y = mouseVec.y;

					if (x != 0 && y != 0 && x != window.Width && y != window.Height)
					{
						try
						{
							Cursors.Default.Draw(g, new System.Drawing.Rectangle(new System.Drawing.Point(x, y), Cursors.Default.Size));
						}
						catch (Exception e)
						{
							Logger.WriteLine($"Exception while drawing mouse. (Checking if window still exists): {e}");
							CheckIfWindowExists(hWnd);
						}
					}
				}

				if (token.IsCancellationRequested)
					return;
			}
		}

		/// <summary>
		/// Periodically sets the foreground window to the desktop.
		/// Windows will do nothing unless we are the foreground window, so this isn't particularly useful.
		/// HooksCPP will automatically set the desktop to foreground if a game brings itself to the foreground.
		/// </summary>
		/// <param name="token"></param>
		private void AutoUnfocusTask(CancellationToken token)
		{
			while (true)
			{
				Thread.Sleep(3000);

				WinApi.SetForegroundWindow((int)WinApi.GetDesktopWindow());

				foreach (Window window in windows.Values)
				{
					window.HooksCPPNamedPipe?.WriteMessage(0x05, 0, 0);
				}

				if (token.IsCancellationRequested)
					return;

			}
		}
		#endregion

		/// <summary>
		/// Checks if a window still exists, and removes it from windows and devicesToWindows if it doesn't.
		/// </summary>
		/// <param name="hWnd"></param>
		public void CheckIfWindowExists(IntPtr hWnd)
		{
			if (!WinApi.IsWindow(hWnd))
			{
				Logger.WriteLine($"Removing hWnd {hWnd}");

				windows.Remove(hWnd);

				InitDeviceToWindows();
			}
		}

		/// <summary>
		/// Called whenever the foreground window changes.
		/// Used for setting up input devices in the Current window tab.
		/// </summary>
		/// <param name="hWinEventHook"></param>
		/// <param name="eventType"></param>
		/// <param name="hWnd"></param>
		/// <param name="idObject"></param>
		/// <param name="idChild"></param>
		/// <param name="dwEventThread"></param>
		/// <param name="dwmsEventTime"></param>
		private void EVENT_SYSTEM_FOREGROUND_Proc(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (IsRunningInSplitScreen)
				return;

			IntPtr our_hWnd = Program.Form_hWnd;
			desktop_hWnd = WinApi.GetDesktopWindow();
			string title = WinApi.GetWindowText(hWnd);
			Logger.WriteLine($"Activated hWnd {hWnd}, self = {our_hWnd == hWnd}, Title = {title}");

			//"Task Switching" is alt+tab and "Cortana" is the start menu.
			if (our_hWnd != hWnd && desktop_hWnd != hWnd && !string.IsNullOrWhiteSpace(title) && title != "Task Switching" && title != "Cortana")
			{
				WinApi.GetWindowThreadProcessId(hWnd, out int pid);
				if (pid != Process.GetCurrentProcess().Id)
				{
					active_hWnd = hWnd;
					Program.Form.WindowTitleText = title;
					Program.Form.WindowHandleText = hWnd.ToString();

					if (windows.TryGetValue(hWnd, out var x))
					{
						//We have devices attached to this window, so display the handles.
						Program.Form.MouseHandleText = x.MouseAttached.ToString();
						Program.Form.KeyboardHandleText = x.KeyboardAttached.ToString();
						Program.Form.ControllerSelectedIndex = x.ControllerIndex;
					}
					else
					{
						//We have no devices attached to this window, so make sure handles display 0.
						Program.Form.MouseHandleText = "0";
						Program.Form.KeyboardHandleText = "0";
						Program.Form.ControllerSelectedIndex = 0;
					}
				}
			}

			if (IsRunningInSplitScreen)
			{
				if (our_hWnd == hWnd)
					InputDisabler.Unlock();
				else
					InputDisabler.Lock();
			}
		}
	}
}
