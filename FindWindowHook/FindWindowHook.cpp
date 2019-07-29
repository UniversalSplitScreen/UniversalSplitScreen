#include "stdafx.h"
#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Windows.h>

NTSTATUS installHook(LPCSTR moduleHandle, std::string lpProcName, void* InCallback)
{
	HOOK_TRACE_INFO hHook = { NULL };

	NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(moduleHandle), lpProcName.c_str()),
		InCallback,
		NULL,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully installed hook " << lpProcName << " in module '" << moduleHandle << "'\n";
	}
	else
	{
		std::string msg = "Failed to install hook " + lpProcName + " in module '" + moduleHandle + "', NTSTATUS: " + std::to_string(hookResult);
		std::cerr << msg << std::endl;
		MessageBox(NULL, msg.c_str(), "Error", MB_OK);
	}

	return hookResult;
}

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

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "FindWindowHook NativeInjectionEntryPoint" << std::endl;
	installHook("user32.dll", "FindWindowA", FindWindow_Hook);
	installHook("user32.dll", "FindWindowW", FindWindow_Hook);
	installHook("user32.dll", "FindWindowExA", FindWindowEx_Hook);
	installHook("user32.dll", "FindWindowExW", FindWindowEx_Hook);
}