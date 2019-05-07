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

static LRESULT CALLBACK MouseProc(_In_ int nCode, _In_ WPARAM wParam, _In_ LPARAM lParam)
{
	ofstream logging;
	logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x64\\Debug\\InjectorCPP_Output.txt", std::ios_base::app);
	logging << "Received code = "<< nCode << endl;
	logging.close();

	//return CallNextHookEx(NULL, nCode, wParam, lParam);
	return 1;
}

DWORD WINAPI HookStart(LPVOID lpParam)
{
	HINSTANCE hmod = *reinterpret_cast<HINSTANCE *>(lpParam);

	ofstream logging;
	logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x64\\Debug\\InjectorCPP_Output.txt", std::ios_base::app);
	logging << "HookStart, hmod="<<hmod << endl;
	logging.close();

	

	HHOOK hhook = SetWindowsHookEx(WH_MOUSE, (HOOKPROC)MouseProc, hmod, 0);
	

	MSG message;
	while (GetMessage(&message, NULL, 0, 0)) {
		TranslateMessage(&message);
		DispatchMessage(&message);
	}

	UnhookWindowsHookEx(hhook);
	return 0;
}

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath32, WCHAR* injectionDllPath64, HWND hWnd, char* ipcChannelName, HINSTANCE hmod)
{	
	UserData* data = new UserData();
	data->hWnd = hWnd;
	strcpy_s(data->ipcChannelName, ipcChannelName);
		
	NTSTATUS nt = RhInjectLibrary(
		pid,
		0,
		EASYHOOK_INJECT_DEFAULT,
		injectionDllPath32,
		injectionDllPath64,
		data,
		sizeof(UserData)
	);

	ofstream logging;
	logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x64\\Debug\\InjectorCPP_Output.txt");
	logging << "START. hmod = " << hmod << "lib32="<< injectionDllPath32 << ",lib64="<< injectionDllPath64 << endl;
	logging.close();

	HANDLE hThread;
	DWORD dwThread;
	hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)HookStart, (LPVOID)hmod, NULL, &dwThread);
	if (hThread)
		return WaitForSingleObject(hThread, INFINITE);
	else
		return 1;

	return nt;//NTSTATUS: 32-bit
}