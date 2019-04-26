using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.SendInput
{
	[StructLayout(LayoutKind.Sequential)]
	public class KBDLLHOOKSTRUCT
	{
		public int vkCode;
		public int scanCode;
		public int flags;
		public uint time;
		public UIntPtr dwExtraInfo;
	}

	class Intercept
	{
		public static bool IsOn = false;

		private static GetMsgProc _proc = HookCallback;
		private static IntPtr _hookID = IntPtr.Zero;

		public Intercept(uint threadID)
		{
			Console.WriteLine("Intercept activated");
			//SendInput.WinApi.SetCapture(hWnd);

			_hookID = SetHook(_proc, threadID);
			//Application.Run();
			//UnhookWindowsHookEx(_hookID);
		}

		private static IntPtr SetHook(GetMsgProc proc, uint threadID)
		{
			return SetWindowsHookEx(2, proc, Marshal.GetHINSTANCE(typeof(Intercept).Module), threadID);

			/*using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				//return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);//TODO: use module handle for game
				
			}*/
		}

		private delegate IntPtr GetMsgProc(int nCode, IntPtr wParam, IntPtr lParam);

		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{

			Console.WriteLine($"INTERCEPT: vkey={wParam}, lParam={lParam}, nCode={nCode}");
			return (IntPtr)1;



			/*
			var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

			//.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

			IntPtr ret = (IntPtr)1;
			//IntPtr ret = (((int)wParam == 0x0104 || (int)wParam == 0x0105) && IsOn && kb.vkCode != Core.Options.CurrentOptions.EndVKey) ? (IntPtr)1 : IntPtr.Zero;
			Console.WriteLine($"INTERCEPT: wParam=0x{wParam:x}, ncode={nCode}, vkey={kb.vkCode}, ret={ret}, dwExtraInfo={kb.dwExtraInfo}, time={kb.time}");
			return ret;

			//return (IsOn && kb.vkCode != Core.Options.CurrentOptions.EndVKey) ? (IntPtr)1 : IntPtr.Zero;				 
			*/



			//Console.WriteLine($"MP: {nCode}, {wParam}, {lParam}");

			/*if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
			{
				MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
			}*/

			//TODO: re-enable if needed


			//return IsOn ? new IntPtr(1) : IntPtr.Zero;


			//return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		private const int WH_MOUSE_LL = 14;
		private const int WH_GETMESSAGE = 3;
		private const int WH_CALLWNDPROC = 4;
		private const int WH_KEYBOARD_LL = 13;
		private const int WH_SYSMSGFILTER = 6;
		private const int WH_MSGFILTER = -1;





		private enum MouseMessages
		{
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct MSLLHOOKSTRUCT
		{
			public POINT pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, GetMsgProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}
