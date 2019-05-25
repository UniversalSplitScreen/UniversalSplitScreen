#pragma once

#include "stdafx.h"
#include <winternl.h>

/*enum OBJECT_INFORMATION_CLASS
{
	ObjectBasicInformation,
	ObjectNameInformation,
	ObjectTypeInformation,
	ObjectAllInformation,
	ObjectDataInformation
};

struct OBJECT_NAME_INFORMATION
{
	UNICODE_STRING Name; // defined in winternl.h
	WCHAR NameBuffer;
};

typedef NTSTATUS(NTAPI* t_NtQueryObject)(HANDLE Handle, OBJECT_INFORMATION_CLASS Info, PVOID Buffer, ULONG BufferSize, PULONG ReturnLength);*/

extern "C" __declspec(dllexport) int SourceEngineUnlock(int pid);
