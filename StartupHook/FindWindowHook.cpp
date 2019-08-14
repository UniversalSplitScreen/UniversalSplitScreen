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

BOOL WINAPI EnumWindows_Hook(WNDENUMPROC lpEnumFunc, LPARAM lParam)
{
	//Pretend it succeeded
	return TRUE;
}

void installFindWindowHooks()
{
	installHook("user32.dll", "FindWindowA", FindWindow_Hook);
	installHook("user32.dll", "FindWindowW", FindWindow_Hook);
	installHook("user32.dll", "FindWindowExA", FindWindowEx_Hook);
	installHook("user32.dll", "FindWindowExW", FindWindowEx_Hook);

	installHook("kernel32.dll", "OpenProcess", OpenProcess_Hook);

	installHook("user32.dll", "EnumWindows", EnumWindows_Hook);
}