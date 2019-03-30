using System;

namespace GetForegroundWindowHook
{
	public class ServerInterface : MarshalByRefObject
	{
		private bool shouldExit = false;
		private IntPtr hWnd;

		public void SetToReleaseHook()
		{
			shouldExit = true;
		}

		public bool ShouldReleaseHook()
		{
			return shouldExit;
		}

		public void SetGame_hWnd(IntPtr hWnd)
		{
			this.hWnd = hWnd;
		}

		public IntPtr GetGame_hWnd()
		{
			return hWnd;
		}

		public void IsInstalled(int clientPID)
		{
			Console.WriteLine("Injected GetForegroundWindow hook into process {0}.\r\n", clientPID);
		}

		public void ReportMessage(string message)
		{
			Console.WriteLine(message);
		}

		public void Ping()
		{

		}
	}
}
