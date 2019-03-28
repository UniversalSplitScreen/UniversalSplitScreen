using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GetRawInputDataHook
{
	public class InjectionEntryPoint : EasyHook.IEntryPoint
	{
		private static EasyHook.LocalHook getRawInputDataHook;

		/*[DllImport("user32.dll")]
		static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out IntPtr pData, ref uint pcbSize, int cbSizeHeader);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate uint GetRawInputDataDelegate(IntPtr hRawInput, DataCommand uiCommand, out IntPtr pData, ref uint pcbSize, int cbSizeHeader);*/


		[DllImport("user32.dll")]
		public static extern uint GetRawInputData(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
		delegate uint GetRawInputDataDelegate(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);


		ServerInterface _server = null;
		
		public InjectionEntryPoint(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			_server = EasyHook.RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
			_server.Ping();
		}

		public void Run(EasyHook.RemoteHooking.IContext context, string channelName)
		{
			int pid = EasyHook.RemoteHooking.GetCurrentProcessId();
			_server.IsInstalled(pid);
			
			getRawInputDataHook = EasyHook.LocalHook.Create(
					EasyHook.LocalHook.GetProcAddress("user32.dll", "GetRawInputData"),
					new GetRawInputDataDelegate(GetRawInputDataHook),
					this);

			getRawInputDataHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

			_server.ReportMessage($"Installed GetRawInputData hook on {pid}");

			EasyHook.RemoteHooking.WakeUpProcess();//TODO: is this required?

			try
			{
				while (true)
				{
					System.Threading.Thread.Sleep(1000);
					_server.Ping();
				}
			}
			catch
			{

			}

			ReleaseHooks();
		}

		private static void ReleaseHooks()
		{
			getRawInputDataHook?.Dispose();

			EasyHook.LocalHook.Release();
		}

		/*public uint GetRawInputDataHook(IntPtr hRawInput, DataCommand uiCommand, [Out] IntPtr pData, ref uint pcbSize, int cbSizeHeader)
		{
			//return 0xFFFFFFFF;
			_server.ReportMessage($"hrawinput={hRawInput}, uiCommand={uiCommand}, pData={pData}, pcbSize={pcbSize}, cbsizeheader={cbSizeHeader}");

			IntPtr pid = (IntPtr)this.pid;

			IntPtr allowed = _server.GetAllowed_hRawInput(pid);
			if (allowed == hRawInput)
			{
				return GetRawInputData(hRawInput, uiCommand, pData, ref pcbSize, cbSizeHeader);
			}

			//_server.ReportMessage($"exit {hRawInput}");

			//_server.ReportMessage("ret -1");
			//pData = new IntPtr(1);
			pcbSize = 0;
			
			return 0xFFFFFFFF;
			//return GetRawInputData(IntPtr.Zero, uiCommand, pData, ref pcbSize, cbSizeHeader);
		}*/

		public uint GetRawInputDataHook(IntPtr hRawInput, DataCommand uiCommand, out RAWINPUT pData, ref uint pcbSize, int cbSizeHeader)
		{			
			IntPtr allowed = _server.GetAllowed_hRawInput();
			if (allowed == hRawInput)
			{
				return GetRawInputData(hRawInput, uiCommand, out pData, ref pcbSize, cbSizeHeader);
			}
			
			pData = default(RAWINPUT);
			return 0xFFFFFFFF;
		}
	}
}
