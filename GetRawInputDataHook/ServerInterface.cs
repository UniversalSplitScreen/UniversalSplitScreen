using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRawInputDataHook
{
    public class ServerInterface : MarshalByRefObject
    {
		private IntPtr allowed_hDevice = IntPtr.Zero;//create two for kb/mouse
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
			Console.WriteLine("Injected hook(s) into process {0}.\r\n", clientPID);
		}

		public void ReportMessage(string message)
		{
			Console.WriteLine(message);
		}

		public void SetAllowed_hDevice(IntPtr hRawInput)
		{
			allowed_hDevice = hRawInput;
		}

		public IntPtr GetAllowed_hDevice()
		{
			return allowed_hDevice;
		}

		public void Ping()
		{

		}
	}
}
