using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRawInputDataHook
{
    public class ServerInterface : MarshalByRefObject
    {
		private IntPtr allowed_hRawInput = IntPtr.Zero;

		public void IsInstalled(int clientPID)
		{
			Console.WriteLine("Injected GetRawInputDataHook into process {0}.\r\n", clientPID);
		}

		public void ReportMessage(string message)
		{
			Console.WriteLine(message);
		}

		public void SetAllowed_hRawInput(IntPtr hRawInput)
		{
			allowed_hRawInput = hRawInput;
		}

		public IntPtr GetAllowed_hRawInput()
		{
			return allowed_hRawInput;
		}

		public void Ping()
		{

		}
	}
}
