#pragma once

#include "stdafx.h"

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath32, WCHAR* injectionDllPath64, HWND hWnd, char* ipcChannelName, bool controllerIndex, int allowedMouseHandle, bool useLegacyInput,
	bool HookGetCursorPos, bool HookGetForegroundWindow, bool HookGetAsyncKeyState, bool HookGetKeyState, bool HookCallWindowProcW, bool HookRegisterRawInputDevices, bool HookSetCursorPos, bool HookXInput);