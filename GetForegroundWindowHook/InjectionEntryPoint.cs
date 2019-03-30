using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GetForegroundWindowHook
{
	public class InjectionEntryPoint : EasyHook.IEntryPoint
	{
		private static IntPtr hWnd = IntPtr.Zero;
		
		#region GetForegroundWindow hook
		private static EasyHook.LocalHook getForegroundWindowHook;

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public IntPtr GetForegroundWindowHook()
		{
			//_server.ReportMessage("GetForegroundWindowHook called");

			//IntPtr actual = GetForegroundWindow();

			//IntPtr actual = GetForegroundWindow();
			//_server.ReportMessage($"game={hWnd}, actual={actual}");

			return hWnd;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate IntPtr GetForegroundWindowDelegate();
		#endregion

		ServerInterface _server = null;

		public InjectionEntryPoint(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			_server = EasyHook.RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
			_server.ReportMessage("test0");
			_server.Ping();
		}

		public void Run(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			int pid = EasyHook.RemoteHooking.GetCurrentProcessId();
			_server.IsInstalled(pid);

			hWnd = _server.GetGame_hWnd();
			//_server.ReportMessage($"hWnd for hook = {hWnd}");

			try
			{
				_server.ReportMessage("test1");
				/*getForegroundWindowHook = EasyHook.LocalHook.Create(
					EasyHook.LocalHook.GetProcAddress("user32.dll", "GetForegroundWindow"),
					new GetForegroundWindowDelegate(GetForegroundWindowHook),
					this);*/
					

				getForegroundWindowHook = EasyHook.LocalHook.CreateUnmanaged(
					EasyHook.LocalHook.GetProcAddress("user32.dll", "GetForegroundWindow"),
					Marshal.GetFunctionPointerForDelegate(new GetForegroundWindowDelegate(GetForegroundWindowHook)),
					IntPtr.Zero);

				
				getForegroundWindowHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });



				_server.ReportMessage($"Installed GetForegroundWindow hook on {pid}");

				//EasyHook.RemoteHooking.WakeUpProcess();//TODO: is this required?
			}
			catch (Exception e)
			{
				_server.ReportMessage($"Error installing GetForegroundWindow hook: {e.ToString()}");
			}

			try
			{
				while (true)
				{
					System.Threading.Thread.Sleep(1000);
					_server.Ping();

					if (_server.ShouldReleaseHook())
						break;
				}
			}
			catch
			{

			}

			ReleaseHooks();
		}

		private void ReleaseHooks()
		{
			_server.ReportMessage("Releasing GetForegroundWindow hook");
			
			getForegroundWindowHook?.Dispose();

			EasyHook.LocalHook.Release();
		}
	}
}
