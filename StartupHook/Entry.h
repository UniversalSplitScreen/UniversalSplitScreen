#pragma once
#include "stdafx.h"
#include <easyhook.h>

// ReSharper disable CppInconsistentNaming
extern "C" void __declspec(dllexport) __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo);
// ReSharper restore CppInconsistentNaming
