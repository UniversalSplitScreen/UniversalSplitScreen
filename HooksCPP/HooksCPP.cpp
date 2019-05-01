#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Windows.h>

HWND hWnd = 0;
std::string _ipcChannelName;
static int x;
static int y;

BOOL WINAPI GetCursorPos_Hook(LPPOINT lpPoint)
{
	POINT p = POINT();
	p.x = x;
	p.y = y;
	ClientToScreen(hWnd, &p);
	*lpPoint = p;
	return true;
}



inline int bytesToInt(BYTE* bytes)
{
	return (int)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);

	/*BYTE *p = bytes;
	int t = 0;
	BYTE* ptr1 = (BYTE*)&t;

	for (int i = 0; i < 4; i++)
	{
		*ptr1 = *p;
		ptr1++;
		p++;
	}

	return t;*/

	//return (int)(*p << 24 | *++p << 16 | *++p << 8 | *++p);

	//int val;
	//std::memcpy(&val, &bytes[offset], sizeof(int));
	//return val;

	//BYTE *p = &bytes[offset];
	//return (int)(*p);
}

void startPipe()
{
	char _pipeNameChars[256];
	sprintf_s(_pipeNameChars, "\\\\.\\pipe\\%s", _ipcChannelName.c_str());

	HANDLE pipe = CreateFile(
		_pipeNameChars,
		GENERIC_READ,
		FILE_SHARE_READ | FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL
	);

	if (pipe == INVALID_HANDLE_VALUE)
	{
		std::cout << "Failed to connect to pipe\n";
		return;
	}

	std::cout << "Connected to pipe\n";

	for (;;)
	{
		BYTE buffer[9];
		DWORD bytesRead = 0;

		BOOL result = ReadFile(
			pipe,
			buffer,
			9 * sizeof(BYTE),
			&bytesRead,
			NULL
		);

		if (result && bytesRead == 9)
		{
			int param1 = bytesToInt(&buffer[1]);

			int param2 = bytesToInt(&buffer[5]);

			//std::cout << "Received message. Msg=" << (int)buffer[0] << ", param1=" << param1 << ", param2=" << param2 << "\n";

			switch (buffer[0])
			{
				case 0x01:
				{
					x = param1;
					y = param2;
					break;
				}
				default:
				{
					break;
				}
			}
		}
		else
		{
			std::cout << "Failed to read message\n";
		}
	}
}

void installHook(LPCSTR moduleHandle, LPCSTR lpProcName, void* InCallback)
{
	HOOK_TRACE_INFO hHook = { NULL };

	NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(moduleHandle), lpProcName),
		InCallback,
		NULL,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully install hook " << lpProcName << "\n";
	}
	else
	{
		std::cout << "Failed install hook " << lpProcName << "\n";
	}
}

struct UserData
{
	HWND hWnd;
	char ipcChannelName[256];
};

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	//Cout will go to the games console
	std::cout << "Injected CPP\n";
	std::cout << "Injected by host process ID: " << inRemoteInfo->HostPID << "\n";
	std::cout << "Passed in data size:" << inRemoteInfo->UserDataSize << "\n";

	if (inRemoteInfo->UserDataSize == sizeof(UserData))
	{
		//Get UserData
		UserData userData = *reinterpret_cast<UserData *>(inRemoteInfo->UserData);

		hWnd = userData.hWnd;
		std::cout << "Received hWnd: " << hWnd << "\n";

		std::string ipcChannelName(userData.ipcChannelName);
		_ipcChannelName = ipcChannelName;
		std::cout << "Received IPC channel: " << ipcChannelName << "\n";
		
		//Install hooks
		installHook(TEXT("user32"), "GetCursorPos", GetCursorPos_Hook);


		//Start named pipe client
		startPipe();
	}
	else
	{
		std::cout << "Failed getting user data\n";
	}

	return;
}