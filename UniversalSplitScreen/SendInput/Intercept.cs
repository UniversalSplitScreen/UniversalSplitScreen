using System;
using System.Runtime.InteropServices;
using UniversalSplitScreen.Core;

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
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, GetMsgProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		public static bool InterceptEnabled = false;

		private static GetMsgProc mouseProc = MouseHookCallback;
		private static IntPtr mouseHookID = IntPtr.Zero;

		private static GetMsgProc keyboardProc = KeyboardHookCallback;
		private static IntPtr keyboardHookID = IntPtr.Zero;
		
		private const int WH_MOUSE_LL = 14;
		private const int WH_KEYBOARD_LL = 13;
		
		private static readonly ushort[] bannedVkeysList = {
			0x03,//VK_CANCEL
			0x5B,//VK_LWIN
			0x5C,//VK_RWIN
			0x5D,//VK_APPS
		};

		public Intercept()
		{
			Array.Sort(bannedVkeysList);

			mouseHookID = SetHook(mouseProc, WH_MOUSE_LL);
			keyboardHookID = SetHook(keyboardProc, WH_KEYBOARD_LL);
			Logger.WriteLine("Intercept keyboard and mouse activated");
		}

		private delegate IntPtr GetMsgProc(int nCode, IntPtr wParam, IntPtr lParam);

		private static IntPtr SetHook(GetMsgProc proc, int hookID)
		{
			return SetWindowsHookEx(hookID, proc, Marshal.GetHINSTANCE(typeof(Intercept).Module), 0);
		}

		private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			return InterceptEnabled ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
		}

		private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (!InterceptEnabled) return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

			var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

			int vk = kb.vkCode;

			if ((vk == 0x09 || vk == 0x1B) //tab or escape
			    && (kb.flags & 0b100000) != 0)//is alt down
			{
				return (IntPtr)1;//alt+tab and alt+esc will change foreground window.
			}

			for (ushort i = 0; i < bannedVkeysList.Length; i++)
			{
				ushort bvk = bannedVkeysList[i];
				if (bvk == vk) return (IntPtr)1;
				else if (vk > bvk) break;
			}

			//Ctrl+esc
			if (vk == 0x1B && GetAsyncKeyState(0x11) != 0)
				return (IntPtr)1;

			return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
		}
	}
}
