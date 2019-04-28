#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Windows.h>

BOOL WINAPI GetCursorPos_Hook(LPPOINT lpPoint)
{
	POINT x = POINT();
	x.x = 250;
	x.y = 250;
	*lpPoint = x;
	return true;
}

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "Injected CPP\n";

	HOOK_TRACE_INFO hHook = { NULL };

	NTSTATUS result = LhInstallHook(
		GetProcAddress(GetModuleHandle(TEXT("user32")), "GetCursorPos"),
		GetCursorPos_Hook,
		NULL,
		&hHook);

	if (!FAILED(result))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
	}

	return;
}