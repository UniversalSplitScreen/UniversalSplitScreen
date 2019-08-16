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
 *	byte 4 : if the find mutex/event/semaphore hooks are enabled
 *	byte 5,6,7,8 : length of data (bytes) of targets (call this X)
 *	bytes 9 to 9 + X : bytes of targets (UTF-16)
 */

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "StartupHook NativeInjectionEntryPoint" << std::endl;

	//const int userDataSize = 64;//TODO: needs increasing

	//if (inRemoteInfo->UserDataSize != userDataSize)
	//	MessageBox(nullptr, "UserData incorrect size", "Error", MB_OK);

	BYTE* data = inRemoteInfo->UserData;
	const bool dinputHookEnabled = data[0] == 1;
	const bool findWindowHookEnabled = data[1] == 1;
	const auto controllerIndex = data[2];
	const bool needWakeUpProcess = data[3] == 1;
	const bool needFindMutexHooks = data[4] == 1;

	if(dinputHookEnabled)
		installDirectInputHooks(controllerIndex);

	if (findWindowHookEnabled)
		installFindWindowHooks();

	if (needFindMutexHooks)
	{
		const size_t targetsLength = (data[5] << 24) + (data[6] << 16) + (data[7] << 8) + data[8];
		auto targets = static_cast<PWSTR>(malloc(targetsLength + sizeof(WCHAR)));
		memcpy(targets, &data[9], targetsLength);
		targets[targetsLength/sizeof(WCHAR)] = '\0';
		installFindMutexHooks(targets);
	}

	if (needWakeUpProcess)
	{
		RhWakeUpProcess();
	}
}