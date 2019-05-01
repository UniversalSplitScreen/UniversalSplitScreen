// InjectorCPP.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <cstring>
#include <easyhook.h>

using namespace std;

struct UserData
{
	HWND hWnd;
	char ipcChannelName[256];//Name will be 30 characters
};

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath, HWND hWnd, char* ipcChannelName)
{	
	UserData* data = new UserData();
	data->hWnd = hWnd;
	strcpy_s(data->ipcChannelName, ipcChannelName);

	NTSTATUS nt = RhInjectLibrary(
		pid,
		0,
		EASYHOOK_INJECT_DEFAULT,
		injectionDllPath,
		NULL,
		data,
		sizeof(UserData)
	);

	return nt;//NTSTATUS: 32-bit
}