#pragma once

#include "stdafx.h"

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath32, WCHAR* injectionDllPath64, HWND hWnd, char* ipcChannelName, HINSTANCE hmod);