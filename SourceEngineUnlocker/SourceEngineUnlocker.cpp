// SourceEngineUnlocker.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <process.h>
#include <winternl.h>
#include <atlstr.h>
#include <cassert>

typedef struct _SYSTEM_HANDLE
{
	DWORD	ProcessID;
	WORD	HandleType;
	WORD	HandleNumber;
	DWORD	KernelAddress;
	DWORD	Flags;
} SYSTEM_HANDLE;

typedef struct _SYSTEM_HANDLE_INFORMATION
{
	DWORD			Count;
	SYSTEM_HANDLE	Handles[1];
} SYSTEM_HANDLE_INFORMATION;

namespace objectinfo
{
	typedef enum M_OBJECT_INFORMATION_CLASS {
		ObjectBasicInformation = 0,
		ObjectNameInformation = 1,
		ObjectTypeInformation = 2,
		ObjectAllInformation = 3,
		ObjectDataInformation = 4
	} MOBJECT_INFORMATION_CLASS;
}

struct OBJECT_NAME_INFORMATION
{
	UNICODE_STRING Name; // defined in winternl.h
	WCHAR NameBuffer;
};

//https://stackoverflow.com/questions/65170/how-to-get-name-associated-with-open-handle
typedef NTSTATUS(NTAPI* t_NtQueryObject)(HANDLE Handle, objectinfo::MOBJECT_INFORMATION_CLASS Info, PVOID Buffer, ULONG BufferSize, PULONG ReturnLength);

//https://stackoverflow.com/questions/65170/how-to-get-name-associated-with-open-handle
t_NtQueryObject NtQueryObject()
{
	static t_NtQueryObject f_NtQueryObject = NULL;
	if (!f_NtQueryObject)
	{
		HMODULE h_NtDll = GetModuleHandle("Ntdll.dll"); // Ntdll is loaded into EVERY process!
		f_NtQueryObject = (t_NtQueryObject)GetProcAddress(h_NtDll, "NtQueryObject");
	}
	return f_NtQueryObject;
}

//https://stackoverflow.com/questions/65170/how-to-get-name-associated-with-open-handle
DWORD GetHandleName(HANDLE h_File, CString* ps_NTPath, objectinfo::MOBJECT_INFORMATION_CLASS xx)
{
	if (h_File == 0 || h_File == INVALID_HANDLE_VALUE)
		return ERROR_INVALID_HANDLE;

	// NtQueryObject() returns STATUS_INVALID_HANDLE for Console handles
	if (((((ULONG_PTR)h_File) & 0x10000003) == 0x3))
	{
		return -1;
	}

	BYTE  u8_Buffer[2000];
	memset(&u8_Buffer[0], 0, 2000);
	DWORD u32_ReqLength = 0;

	UNICODE_STRING* pk_Info = &((OBJECT_NAME_INFORMATION*)u8_Buffer)->Name;
	pk_Info->Buffer = 0;
	pk_Info->Length = 0;

	NtQueryObject()(h_File, xx, u8_Buffer, sizeof(u8_Buffer), &u32_ReqLength);

	// On error pk_Info->Buffer is NULL
	if (!pk_Info->Buffer || !pk_Info->Length)
		return ERROR_FILE_NOT_FOUND;

	pk_Info->Buffer[pk_Info->Length / 2] = 0; // Length in Bytes!

	*ps_NTPath = pk_Info->Buffer;
	return 0;
}

typedef NTSTATUS(NTAPI* lpNtDuplicateObject)(HANDLE SourceProcessHandle, HANDLE SourceHandle, HANDLE TargetProcessHandle, PHANDLE TargetHandle, ACCESS_MASK DesiredAccess, ULONG Attributes, ULONG Options);
static lpNtDuplicateObject NtDuplicateObject = nullptr;

//TODO: NEEDS CREDIT IN ABOUT PAGE
//https://www.codeguru.com/cpp/w-p/system/processesmodules/article.php/c2827/Examine-Information-on-Windows-NT-System-Level-Primitives.htm
int Close(DWORD m_processId)
{
	DWORD size = 0x2000;
	DWORD needed = 0;
	DWORD i = 0;
	BOOL  ret = 0;
	
	HMODULE hNtdll = LoadLibraryA("ntdll");
	NtDuplicateObject = (lpNtDuplicateObject)GetProcAddress(hNtdll, "NtDuplicateObject");

	// Allocate the memory for the buffer
	SYSTEM_HANDLE_INFORMATION* pSysHandleInformation = (SYSTEM_HANDLE_INFORMATION*)VirtualAlloc(NULL, size, MEM_COMMIT, PAGE_READWRITE);

	if (pSysHandleInformation == NULL)
		return -1;

	// Query the needed buffer size for the objects ( system wide )
	if (NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)16, pSysHandleInformation, size, &needed) != 0)
	{
		if (needed == 0)
		{
			ret = -2;
			goto cleanup;
		}

		// The size was not enough
		VirtualFree(pSysHandleInformation, 0, MEM_RELEASE);

		pSysHandleInformation = (SYSTEM_HANDLE_INFORMATION*)VirtualAlloc(NULL, size = needed + 256, MEM_COMMIT, PAGE_READWRITE);
	}

	if (pSysHandleInformation == NULL)
		return -3;

	// Query the objects ( system wide )
	if (NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)16, pSysHandleInformation, size, NULL) != 0)
	{
		ret = -4;
		goto cleanup;
	}

	// Iterating through the objects
	for (i = 0; i < pSysHandleInformation->Count; i++)
	{
		// ProcessId filtering check
		if (pSysHandleInformation->Handles[i].ProcessID == m_processId)
		{
			
			HANDLE hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, m_processId);
			HANDLE handle = (HANDLE)(pSysHandleInformation->Handles[i].HandleNumber);

			HANDLE hHandleLocal = NULL;
			NTSTATUS status = NtDuplicateObject(hProcess, handle, GetCurrentProcess(), &hHandleLocal, 0, 0, DUPLICATE_SAME_ACCESS);// | DUPLICATE_SAME_ATTRIBUTES);

			CString path;
			
			DWORD _ret = GetHandleName(hHandleLocal, &path, objectinfo::ObjectNameInformation);
			CloseHandle(hHandleLocal);
			if (_ret == 0)
			{
				if (path.Find("hl2_singleton_mutex") != -1)
				{
					pSysHandleInformation->Handles[i].HandleType = (WORD)(pSysHandleInformation->Handles[i].HandleType % 256);
						
					HANDLE dummyHandle = NULL;
					BOOL err_ret = DuplicateHandle(hProcess, handle, GetCurrentProcess(), &dummyHandle, 0, FALSE, DUPLICATE_CLOSE_SOURCE | DUPLICATE_SAME_ACCESS);
					CloseHandle(dummyHandle);
					ret = 1;
					goto cleanup;
				}
			}
		}
	}
	
	cleanup:
	if (pSysHandleInformation != NULL)
	{
		VirtualFree(pSysHandleInformation, 0, MEM_RELEASE);
	}

	return ret;
}

extern "C" __declspec(dllexport) int SourceEngineUnlock(int pid)
{
	return Close(pid);
}