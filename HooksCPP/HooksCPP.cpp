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

struct UserData
{
	HWND hWnd;
	char ipcChannelName[256];
	int pipeHandle;
};

inline int bytesToInt(BYTE* bytes, int offset)
{
	//return (int)(bytes[offset] << 24 | bytes[offset+1] << 16 | bytes[offset+2] << 8 | bytes[offset+3]);

	/*BYTE *p = &bytes[offset];
	unsigned int t = *p;
	BYTE* ptr1 = (BYTE*)&t;
	*ptr1 = *p;
	ptr1++;
	p++;
	*ptr1 = *p;
	ptr1++;
	p++;
	*ptr1 = *p;
	ptr1++;
	p++;
	*ptr1 = *p;
	ptr1++;
	p++;

	return t;*/


	//t <<= 24 
	//return (int)(*p << 24 | *++p << 16 | *++p << 8 | *++p);

	//int val;
	//std::memcpy(&val, &bytes[offset], sizeof(int));
	//return val;

	BYTE *p = &bytes[offset];
	return (int)(*p);
}

void startPipe()
{
	std::string pipeName = "\\\\.\\pipe\\" + _ipcChannelName;
	char tmp[256];
	sprintf_s(tmp, "\\\\.\\pipe\\%s", _ipcChannelName.c_str());
	//std::string pipeName = "\\\\.\\pipe\\testpipe";

	HANDLE pipe = CreateFile(
		tmp,
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
			//int param1 = bytesToInt(&buffer[1]);
			//int param1 = (int)(&buffer[1]);
			int param1 = (int)(buffer[1] << 24 | buffer[2] << 16 | buffer[3] << 8 | buffer[4]);
			//int param2 = bytesToInt(&buffer[5]);
			//int param2 = (int)(&buffer[5]);
			int param2 = (int)(buffer[5] << 24 | buffer[6] << 16 | buffer[7] << 8 | buffer[8]);

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
		_ipcChannelName = ipcChannelName;
		std::cout << "Received IPC channel: " << ipcChannelName << "\n";
		
		HOOK_TRACE_INFO hHook = { NULL };

		NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(TEXT("user32")), "GetCursorPos"),
		GetCursorPos_Hook,
		NULL,
		&hHook);

		if (!FAILED(hookResult))
		{
			ULONG ACLEntries[1] = { 0 };
			LhSetExclusiveACL(ACLEntries, 1, &hHook);
		}

		startPipe();
	}
	else

	{
		std::cout << "Failed getting user data\n";
	}


	return;

}