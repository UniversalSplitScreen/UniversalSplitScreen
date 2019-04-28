#pragma once
#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Windows.h>

extern "C" void __declspec(dllexport) __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo);

//extern "C" __declspec(dllexport) void NativeInjectionEntryPoint2(REMOTE_ENTRY_INFO* inRemoteInfo);