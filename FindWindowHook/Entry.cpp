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

	installFindWindowHooks();
	installDirectInputHooks();

	RhWakeUpProcess();
}