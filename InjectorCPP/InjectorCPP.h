#pragma once

#include "stdafx.h"

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath, HWND hWnd, char* ipcChannelName, HINSTANCE hmod);