#include "stdafx.h"
#include "Entry.h"
#include "InstallHooks.h"
#include <easyhook.h>
#include <string>
#include <iostream>

/*	UserData byte array layout:
 *	Byte 0 : 1 if dinput hook is enabled
 *	Byte 1 : 1 if FindWindow hook is enabled
 *	byte 2 : The controllerIndex
 *	byte 3 : If RhWakeUpProcess needs to be called (ie it is suspended)
 */

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "StartupHook NativeInjectionEntryPoint" << std::endl;

	const int userDataSize = 4;

	if (inRemoteInfo->UserDataSize != userDataSize)
		MessageBox(nullptr, "UserData incorrect size", "Error", MB_OK);

	BYTE* data = inRemoteInfo->UserData;
	bool dinputHookEnabled = data[0] == 1;
	bool findWindowHookEnabled = data[1] == 1;
	const auto controllerIndex = data[2];
	bool needWakeUpProcess = data[3] == 1;

	if(dinputHookEnabled)
		installDirectInputHooks(controllerIndex);

	if (findWindowHookEnabled)
		installFindWindowHooks();

	installFindMutexHooks();

	if (needWakeUpProcess)
	{
		RhWakeUpProcess();
	}
}