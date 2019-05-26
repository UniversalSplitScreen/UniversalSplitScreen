#pragma once

#include "stdafx.h"
#include <winternl.h>

extern "C" __declspec(dllexport) int SourceEngineUnlock(int pid);
