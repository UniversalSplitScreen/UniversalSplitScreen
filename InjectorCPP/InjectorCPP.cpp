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
	char ipcChannelNameRead[256];//Name will be 30 characters
	char ipcChannelNameWrite[256];//Name will be 30 characters
	int controllerIndex;
	int allowedMouseHandle;
	bool HookGetCursorPos;
	bool HookGetForegroundWindow;
	bool HookGetAsyncKeyState;
	bool HookGetKeyState;
	bool HookCallWindowProcW;
	bool HookRegisterRawInputDevices;
	bool HookSetCursorPos;
	bool HookXInput;
	bool useLegacyInput;
};

extern "C" __declspec(dllexport) int Inject(
	int pid, 
	WCHAR* injectionDllPath32, 
	WCHAR* injectionDllPath64, 
	HWND hWnd, 
	char* ipcChannelNameRead, 
	char* ipcChannelNameWrite, 
	int controllerIndex, 
	int allowedMouseHandle,
	bool useLegacyInput,
	bool HookGetCursorPos, 
	bool HookGetForegroundWindow, 
	bool HookGetAsyncKeyState,
	bool HookGetKeyState, 
	bool HookCallWindowProcW, 
	bool HookRegisterRawInputDevices, 
	bool HookSetCursorPos, 
	bool HookXInput)
{	
	UserData* data = new UserData();
	data->hWnd = hWnd;
	strcpy_s(data->ipcChannelNameRead, ipcChannelNameRead);
	strcpy_s(data->ipcChannelNameWrite, ipcChannelNameWrite);
	data->controllerIndex = controllerIndex;
	data->allowedMouseHandle = allowedMouseHandle;

	data->HookGetCursorPos = HookGetCursorPos;
	data->HookGetForegroundWindow = HookGetForegroundWindow;
	data->HookGetAsyncKeyState = HookGetAsyncKeyState;
	data->HookGetKeyState = HookGetKeyState;
	data->HookCallWindowProcW = HookCallWindowProcW;
	data->HookRegisterRawInputDevices = HookRegisterRawInputDevices;
	data->HookSetCursorPos = HookSetCursorPos;
	data->HookXInput = HookXInput;
	data->useLegacyInput = useLegacyInput;
		
	NTSTATUS nt = RhInjectLibrary(
		pid,
		0,
		EASYHOOK_INJECT_DEFAULT,
		injectionDllPath32,
		injectionDllPath64,
		data,
		sizeof(UserData)
	);

	return nt;//NTSTATUS: 32-bit
}