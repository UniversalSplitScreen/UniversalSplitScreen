#include "stdafx.h"
#include "Entry.h"
#include "FindWindowHook.h"
#include "DirectInputHook.h"
#include <easyhook.h>
#include <string>
#include <iostream>

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "FindWindowHook NativeInjectionEntryPoint" << std::endl;

	if (inRemoteInfo->UserDataSize == sizeof(int))
	{
		const auto controllerIndex = *reinterpret_cast<int *>(inRemoteInfo->UserData);
		installDirectInputHooks(controllerIndex);
	}
	else
		MessageBox(nullptr, "UserData incorrect size", "Error", MB_OK);

	installFindWindowHooks();

	RhWakeUpProcess();
}