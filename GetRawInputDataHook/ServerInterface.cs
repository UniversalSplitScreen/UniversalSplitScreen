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
		private int cursorX;
		private int cursorY;
		
		private byte[] VKeysPressed = new byte[256];//Mouse VKeys are 0x01 to 0x06

		public void SetToReleaseHook()
		{
			shouldExit = true;
		}
		
		public bool ShouldReleaseHook()
		{
			return shouldExit;
		}

		public void SetCursorPosition(int x, int y)
		{
			cursorX = x;
			cursorY = y;
		}

		public void GetCursorPosition(out int x, out int y)
		{
			x = cursorX;
			y = cursorY;
		}

		public void SetVKey(int VKey, bool isPressed)
		{
			VKeysPressed[VKey] = (byte)(isPressed ? 0b10000000 : 0);
		}

		public bool GetIsVKeyPressed(int VKey)
		{
			return VKeysPressed[VKey] != 0;
		}

		public byte[] GetVKeyArray()
		{
			return VKeysPressed;
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
			VKeysPressed = new byte[256];
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
