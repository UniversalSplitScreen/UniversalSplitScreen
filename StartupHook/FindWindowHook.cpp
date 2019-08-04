#include "stdafx.h"
#include "FindWindowHook.h"
#include "Hooking.h"

//FindWindowA/FindWindowW
HWND WINAPI FindWindow_Hook(LPCSTR lpClassName, LPCSTR lpWindowName)
{
	return NULL;
}

//FindWindowExA/FindWindowExW
HWND WINAPI FindWindowEx_Hook(LPCSTR lpClassName, LPCSTR lpWindowName)
{
	return NULL;
}

HANDLE WINAPI OpenProcess_Hook(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId)
{
	return NULL;
}

BOOL WINAPI RegisterRawInputDevices_Hook(
	PCRAWINPUTDEVICE pRawInputDevices,
	UINT             uiNumDevices,
	UINT             cbSize
)
{
	/*for (UINT i = 0; i < uiNumDevices; i++)
	{
		auto r = pRawInputDevices[i];
		MessageBox(NULL, (std::to_string(r.dwFlags) + ", " + std::to_string((int)r.hwndTarget) + ", " + std::to_string(r.usUsage) + ", " + std::to_string(r.usUsagePage) + ", ").c_str(), "reg0", MB_OK);
	}*/

	auto p = static_cast<tagRAWINPUTDEVICE*>(malloc(uiNumDevices * cbSize));
	//memcpy(p, pRawInputDevices, uiNumDevices * sizeof(tagRAWINPUTDEVICE));

	auto rp = static_cast<PCRAWINPUTDEVICE>(p);
	
	for (UINT i = 0; i < uiNumDevices; i++)
	{
		auto cp = pRawInputDevices[i];

		//cp.dwFlags |= RIDEV_INPUTSINK;
		//cp.dwFlags &= ~(RIDEV_EXINPUTSINK);
		cp.dwFlags = RIDEV_INPUTSINK;

		memcpy(static_cast<void*>(p + i * sizeof(tagRAWINPUTDEVICE)), &cp, cbSize);
	}

	/*for (UINT i = 0; i < uiNumDevices; i++)
	{
		auto r = p[i];
		MessageBox(NULL, (std::to_string(r.dwFlags) + ", " + std::to_string((int)r.hwndTarget) + ", " + std::to_string(r.usUsage) + ", " + std::to_string(r.usUsagePage) + ", ").c_str(), "reg0", MB_OK);
	}*/
	return RegisterRawInputDevices(p, uiNumDevices, cbSize);
}

void installFindWindowHooks()
{
	installHook("user32.dll", "FindWindowA", FindWindow_Hook);
	installHook("user32.dll", "FindWindowW", FindWindow_Hook);
	installHook("user32.dll", "FindWindowExA", FindWindowEx_Hook);
	installHook("user32.dll", "FindWindowExW", FindWindowEx_Hook);

	installHook("kernel32.dll", "OpenProcess", OpenProcess_Hook);

	installHook("User32.dll", "RegisterRawInputDevices", RegisterRawInputDevices_Hook);
}