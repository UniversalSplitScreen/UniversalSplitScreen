// InjectorCPP.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <cstring>
#include <easyhook.h>

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath)
{
	std::cout << "injector cpp";
	DWORD testData = 77;

	NTSTATUS nt = RhInjectLibrary(
		pid,
		0,
		EASYHOOK_INJECT_DEFAULT,
		injectionDllPath,
		NULL,
		&testData,
		sizeof(DWORD)
	);

	return nt;//NTSTATUS: 32-bit
}