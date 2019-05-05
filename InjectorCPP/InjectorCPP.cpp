// InjectorCPP.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <cstring>
#include <easyhook.h>

#include <iostream>
#include <fstream>

using namespace std;

struct UserData
{
	HWND hWnd;
	char ipcChannelName[256];//Name will be 30 characters
};

LRESULT CALLBACK MouseProc(_In_ int nCode, _In_ WPARAM wParam, _In_ LPARAM lParam)
{
	ofstream logging;
	logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\InjectorCPP_Output.txt", std::ios_base::app);
	logging << "Received code = "<< nCode << endl;
	logging.close();


	return 0;
}

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath, HWND hWnd, char* ipcChannelName, HINSTANCE hmod)
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



	//HMODULE _hModule = GetModuleHandle(currentDllPath);
	HHOOK hhook = SetWindowsHookEx(WH_MOUSE_LL, (HOOKPROC)MouseProc, hmod, 0);
	if (hhook == NULL)
	{
		return GetLastError();
	}
	else
	{
		return 0;
	}
	//return (hhook == NULL) ? 123 : 0;

	return nt;//NTSTATUS: 32-bit
}