using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.RawInput;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen.Core
{
	class SplitScreenManager
	{
		public bool IsRunningInSplitScreen { get; private set; } = false;

		Dictionary<Task, CancellationTokenSource> setFocusTasks = new Dictionary<Task, CancellationTokenSource>();
		Dictionary<Task, CancellationTokenSource> drawMouseTasks = new Dictionary<Task, CancellationTokenSource>();

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

		public void Init()
		{
			Console.WriteLine("Registering EVENT_SYSTEM_FOREGROUND hook");
			EVENT_SYSTEM_FOREGROUND_delegate = new WinApi.WinEventDelegate(EVENT_SYSTEM_FOREGROUND_Proc);
			IntPtr m_hhook = WinApi.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, EVENT_SYSTEM_FOREGROUND_delegate, 0, 0, WINEVENT_OUTOFCONTEXT);
		}
		
		public void ActivateSplitScreen()
		{
			Program.Form.WindowState = FormWindowState.Minimized;

			IsRunningInSplitScreen = true;
			InputDisabler.Lock();
			Intercept.IsOn = true;
			deviceToWindows.Clear();

			//Check if windows still exist
			for (int i = 0; i < windows.Count; i++)
			{
				IntPtr hWnd = windows.ElementAt(i).Key;
				if (!WinApi.IsWindow(hWnd))
					windows.Remove(hWnd);
			}

			foreach (var pair in windows)
			{
				IntPtr hWnd = pair.Key;
				Window window = pair.Value;

				Console.WriteLine($"hWnd={hWnd}, mouse={window.MouseAttached}, kb={window.KeyboardAttached}");

				//Initialise deviceToWindows
				if (!deviceToWindows.ContainsKey(window.MouseAttached))
					deviceToWindows[window.MouseAttached] = windows.Values.Where(x => x.MouseAttached == window.MouseAttached).ToArray();

				if (!deviceToWindows.ContainsKey(window.KeyboardAttached))
					deviceToWindows[window.KeyboardAttached] = windows.Values.Where(x => x.KeyboardAttached == window.KeyboardAttached).ToArray();

				//Borderlands 2 requriest WM_INPUT to be sent to a window named DIEmWin, not the main hWnd.
				foreach (ProcessThread thread in Process.GetProcessById(window.pid).Threads)
				{

					int WindowEnum(IntPtr _hWnd, int lParam)
					{
						int threadID = WinApi.GetWindowThreadProcessId(_hWnd, out int pid);
						if (threadID == lParam)
						{
							string windowText = WinApi.GetWindowText(_hWnd);
							Console.WriteLine($" - thread id=0x{threadID:x}, _hWnd=0x{_hWnd:x}, window text={windowText}");

							if (windowText.ToLower().Contains("DIEmWin".ToLower()))//TODO: make configurable
							{
								window.borderlands2_DIEmWin_hWnd = _hWnd;
							}
						}

						return 1;
					}

					WinApi.EnumWindows(WindowEnum, thread.Id);
				}

				//WM_ACTIVATE/WM_SETFOCUS tasks
				if (Options.CurrentOptions.SendWM_ACTIVATE || Options.CurrentOptions.SendWM_SETFOCUS)
				{
					CancellationTokenSource c = new CancellationTokenSource();
					Task task = new Task(() => SetFocus(pair.Key, c.Token), c.Token);
					task.Start();
					setFocusTasks.Add(task, c);
				}

				//Draw mouse tasks
				if (Options.CurrentOptions.DrawMouse)
				{
					CancellationTokenSource c = new CancellationTokenSource();
					Task task = new Task(() => DrawMouse(hWnd, c.Token), c.Token);
					task.Start();
					drawMouseTasks.Add(task, c);
				}

				//EasyHook
				//TODO: REMOVE TRUE
				if (true || Options.CurrentOptions.Hook_FilterRawInput || Options.CurrentOptions.Hook_FilterWindowsMouseInput || Options.CurrentOptions.Hook_GetForegroundWindow)
				{
					string channelName = null;
					var serverChannel_getRawInputData = EasyHook.RemoteHooking.IpcCreateServer<GetRawInputDataHook.ServerInterface>(ref channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton);

					//string channelName = "sstest";
					//var serverChannel = EasyHook.RemoteHooking.IpcCreateServer<GetRawInputDataHook.ServerInterface>(ref channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton, System.Security.Principal.WellKnownSidType.WorldSid);

					//Console.WriteLine($"Channel name = {channelName}, Channel uri = {serverChannel_getRawInputData.GetChannelUri()}");

					var server_getRawInputData = EasyHook.RemoteHooking.IpcConnectClient<GetRawInputDataHook.ServerInterface>(channelName);
					server_getRawInputData.Ping();
					server_getRawInputData.SetGame_hWnd(hWnd);
					server_getRawInputData.SetAllowed_hDevice(window.MouseAttached);

					string injectionLibrary_getRawInputData = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "GetRawInputDataHook.dll");

					try
					{
						// Injecting into existing process by Id
						Console.WriteLine("Attempting to inject (GetRawInputData) into process {0}", window.pid);

						// inject into existing process
						EasyHook.RemoteHooking.Inject(
							window.pid,                         // ID of process to inject into
							injectionLibrary_getRawInputData,   // 32-bit library to inject (if target is 32-bit)
							injectionLibrary_getRawInputData,   // 64-bit library to inject (if target is 64-bit)
							channelName,                        // the parameters to pass into injected library
							Options.CurrentOptions.Hook_FilterRawInput,
							Options.CurrentOptions.Hook_FilterWindowsMouseInput,
							Options.CurrentOptions.Hook_GetForegroundWindow
						);

						window.GetRawInputData_HookIPCServerChannel = serverChannel_getRawInputData;
						window.GetRawInputData_HookServer = server_getRawInputData;
					}
					catch (Exception e)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("There was an error while injecting hook into target:");
						Console.ResetColor();
						Console.WriteLine(e.ToString());
					}
				}
			}

			Program.Form.OnSplitScreenStart();
		}

		public void DeactivateSplitScreen()
		{
			IsRunningInSplitScreen = false;
			InputDisabler.Unlock();
			Intercept.IsOn = false;

			foreach (var thread in setFocusTasks)
				thread.Value.Cancel();

			foreach (var thread in drawMouseTasks)
				thread.Value.Cancel();

			foreach (var window in windows.Values)
			{
				window.GetRawInputData_HookServer?.SetToReleaseHook();
			}

			setFocusTasks.Clear();
			drawMouseTasks.Clear();

			Program.Form.OnSplitScreenEnd();
			Program.Form.WindowState = FormWindowState.Normal;
		}

		public void SetMousePointer(IntPtr mouse)
		{
			if (!windows.ContainsKey(active_hWnd))
				windows[active_hWnd] = new Window(active_hWnd);

			windows[active_hWnd].MouseAttached = mouse;

			Program.Form.MouseHandleText = mouse.ToString();

			if ((int)mouse == 0 && (int)windows[active_hWnd].KeyboardAttached == 0)
				windows.Remove(active_hWnd);
		}

		public void SetKeyboardPointer(IntPtr keyboard)
		{
			if (!windows.ContainsKey(active_hWnd)) windows[active_hWnd] = new Window(active_hWnd);
			windows[active_hWnd].KeyboardAttached = keyboard;

			Program.Form.KeyboardHandleText = keyboard.ToString();

			if ((int)keyboard == 0 && (int)windows[active_hWnd].MouseAttached == 0)
				windows.Remove(active_hWnd);
		}

		public void ResetAllHandles()
		{
			windows.Clear();
			Program.Form.MouseHandleText = "0";
			Program.Form.KeyboardHandleText = "0";
		}

		#endregion

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
							Console.WriteLine($"Exception while drawing mouse. (Checking if window still exists): {e}");
							if (!WinApi.IsWindow(hWnd))
								windows.Remove(hWnd);
						}
					}
				}

				if (token.IsCancellationRequested)
					return;
			}
		}

		private void EVENT_SYSTEM_FOREGROUND_Proc(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (IsRunningInSplitScreen)
				return;

			IntPtr our_hWnd = Program.Form_hWnd;
			desktop_hWnd = WinApi.GetDesktopWindow();
			string title = WinApi.GetWindowText(hWnd);
			Console.WriteLine($"Activated hWnd {hWnd}, self = {our_hWnd == hWnd}, Title = {title}");

			if (our_hWnd != hWnd && desktop_hWnd != hWnd && !string.IsNullOrWhiteSpace(title) && title != "Task Switching" && title != "Cortana")
			{
				active_hWnd = hWnd;
				Program.Form.WindowTitleText = title;
				Program.Form.WindowHandleText = hWnd.ToString();

				if (windows.TryGetValue(hWnd, out var x))
				{
					Program.Form.MouseHandleText = x.MouseAttached.ToString();
					Program.Form.KeyboardHandleText = x.KeyboardAttached.ToString();
				}
				else
				{
					Program.Form.MouseHandleText = "0";
					Program.Form.KeyboardHandleText = "0";
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
