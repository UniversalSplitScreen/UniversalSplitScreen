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
 *	byte 5,6,7,8, 9,10,11,12 : pointer to null terminated wide char string (first 4 bytes are empty on 32 bit)
 */

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "StartupHook NativeInjectionEntryPoint" << std::endl;

	const int userDataSize = 4;//TODO: needs increasing

	if (inRemoteInfo->UserDataSize != userDataSize)
		MessageBox(nullptr, "UserData incorrect size", "Error", MB_OK);

	BYTE* data = inRemoteInfo->UserData;
	bool dinputHookEnabled = data[0] == 1;
	bool findWindowHookEnabled = data[1] == 1;
	const auto controllerIndex = data[2];
	bool needWakeUpProcess = data[3] == 1;
	bool needFindMutexHooks = data[4] == 1;
	needFindMutexHooks = true;//TODO: remove

	if(dinputHookEnabled)
		installDirectInputHooks(controllerIndex);

	if (findWindowHookEnabled)
		installFindWindowHooks();

	if (needFindMutexHooks)
	{
		int offset = 0;
#ifdef X86
		offset = 4;
#endif
		//PWSTR targets = reinterpret_cast<wchar_t *>(&data[5 + offset]);
		installFindMutexHooks(L"Overkill Engine Game&&&&&hl2_singleton_mutex");
	}

	if (needWakeUpProcess)
	{
		RhWakeUpProcess();
	}
}