#include "stdafx.h"
#include "Entry.h"
#include "FindWindowHook.h"
#include "DirectInputHook.h"
#include <easyhook.h>
#include <string>
#include <iostream>

/*	UserData byte array layout:
 *	Byte 0 : 1 if dinput hook is enabled
 *	Byte 1 : 1 if FindWindow hook is enabled
 *	byte 2 : The controllerIndex
 */

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	std::cout << "FindWindowHook NativeInjectionEntryPoint" << std::endl;

	const int userDataSize = 3;

	if (inRemoteInfo->UserDataSize != userDataSize)
		MessageBox(nullptr, "UserData incorrect size", "Error", MB_OK);

	BYTE* data = inRemoteInfo->UserData;
	bool dinputHookEnabled = data[0] == 1;
	bool findWindowHookEnabled = data[1] == 1;
	const auto controllerIndex = data[2];

	if(dinputHookEnabled)
		installDirectInputHooks(controllerIndex);

	if (findWindowHookEnabled)
		installFindWindowHooks();

	RhWakeUpProcess();
}