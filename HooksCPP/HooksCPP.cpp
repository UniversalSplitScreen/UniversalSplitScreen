#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Windows.h>

HWND hWnd = 0;
HANDLE pipeHandle;

BOOL WINAPI GetCursorPos_Hook(LPPOINT lpPoint)
{
	POINT x = POINT();
	x.x = 200;
	x.y = 200;
	ClientToScreen(hWnd, &x);
	*lpPoint = x;
	return true;
}

struct UserData
{
	HWND hWnd;
	char ipcChannelName[30];
	int pipeHandle;
};

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	//Cout will go to the games console
	std::cout << "Injected CPP\n";
	std::cout << "Injected by host process ID: " << inRemoteInfo->HostPID << "\n";
	std::cout << "Passed in data size:" << inRemoteInfo->UserDataSize << "\n";

	if (inRemoteInfo->UserDataSize == sizeof(UserData))
	{
		UserData userData = *reinterpret_cast<UserData *>(inRemoteInfo->UserData);

		hWnd = userData.hWnd;
		std::cout << "Received hWnd: " << hWnd << "\n";

		std::string ipcChannelName(userData.ipcChannelName);
		std::cout << "Received IPC channel: " << ipcChannelName << "\n";

		pipeHandle = (HANDLE)userData.pipeHandle;
		std::cout << "Received pipe handle int: " << userData.pipeHandle << ", HANDLE: " << pipeHandle << "\n";

		DWORD bytesRead;
		BOOL success = FALSE;
		BYTE byteBuffer[9];

		for (;;)
		{
			success = ReadFile(pipeHandle, byteBuffer, 9, &bytesRead, NULL);
			std::cout << "Success=" << success << ", bytesRead=" << bytesRead << "\n";
			if (!success || bytesRead == 0) continue;

			std::cout << "Received bytes: " << byteBuffer[0] << "," << byteBuffer[1] << "," << byteBuffer[2] << "," << byteBuffer[3] << "," << byteBuffer[4] << "," << byteBuffer[5] << "," << byteBuffer[6] << "," << byteBuffer[7] << "," << byteBuffer[8] << "," << byteBuffer[9] << "\n";
		}

	}
	


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