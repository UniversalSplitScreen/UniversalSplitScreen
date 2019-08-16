// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>



// reference additional headers your program requires here

#if _WIN64
#define X64
#else
#define X86
#endif

#ifdef X64
using PtrSize = long long;//64 bit pointers
#else
using PtrSize = int; //32 bit pointers
#endif