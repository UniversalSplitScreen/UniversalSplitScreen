using System;
using System.Runtime.InteropServices;
using System.Linq;
using AutoHotkey.Interop;
using System.Windows.Forms;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen.RawInput
{
	//https://www.codeproject.com/Articles/17123/Using-Raw-Input-from-C-to-handle-multiple-keyboard
	class RawInputManager
	{

		public RawInputManager(IntPtr windowHandle)
		{
			//GetDeviceList();
			Console.WriteLine($"Attempting to RegisterRawInputDevices for window handle {windowHandle}");

			//https://docs.microsoft.com/en-us/windows-hardware/drivers/hid/hidclass-hardware-ids-for-top-level-collections
			RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[2];

			//Keyboard
			rid[0].usUsagePage = 0x01;
			rid[0].usUsage = 0x06;
			rid[0].dwFlags = (uint)RawInputDeviceFlags.RIDEV_INPUTSINK;
			rid[0].hwndTarget = windowHandle;

			//Mouse
			rid[1].usUsagePage = 0x01;
			rid[1].usUsage = 0x02;
			rid[1].dwFlags = (uint)RawInputDeviceFlags.RIDEV_INPUTSINK;
			rid[1].hwndTarget = windowHandle;

			bool success = WinApi.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0]));
			Console.WriteLine($"Succeeded RegisterRawInputDevices Keyboard = {success}");

			if (!success)
			{
				int error = Marshal.GetLastWin32Error();
				Console.WriteLine($"Error code = {error}");
			}

			success = WinApi.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[1]));
			Console.WriteLine($"Succeeded RegisterRawInputDevices Mouse = {success}");

			if (!success)
			{
				int error = Marshal.GetLastWin32Error();
				Console.WriteLine($"Error code = {error}");
			}
		}

		public void GetDeviceList()
		{
			uint numDevices = 0;
			int cbSize = Marshal.SizeOf(typeof(RAWINPUTDEVICELIST));

			if (WinApi.GetRawInputDeviceList(IntPtr.Zero, ref numDevices, (uint)cbSize) == 0)//Return value isn't zero if there is an error
			{
				IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(cbSize * numDevices));
				WinApi.GetRawInputDeviceList(pRawInputDeviceList, ref numDevices, (uint)cbSize);

				for (int i = 0; i < numDevices; i++)
				{
					RAWINPUTDEVICELIST rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(new IntPtr(pRawInputDeviceList.ToInt32() + (cbSize * i)), typeof(RAWINPUTDEVICELIST));

					uint pcbSize = 0;
					WinApi.GetRawInputDeviceInfo(rid.hDevice, 0x2000000b,  IntPtr.Zero, ref pcbSize);//Get the size required in memory
					IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
					WinApi.GetRawInputDeviceInfo(rid.hDevice, 0x2000000b, pData, ref pcbSize);
					var device = (RID_DEVICE_INFO)Marshal.PtrToStructure(pData, typeof(RID_DEVICE_INFO));
					Console.WriteLine($"Found device, {device.dwType}, {device.keyboard.dwNumberOfKeysTotal}");
				}

				Marshal.FreeHGlobal(pRawInputDeviceList);
			}
		}
	}
}
