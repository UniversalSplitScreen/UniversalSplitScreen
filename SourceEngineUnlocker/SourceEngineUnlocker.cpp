// SourceEngineUnlocker.cpp : Defines the exported functions for the DLL application.
//

/*#include "stdafx.h"
#include <Windows.h>
#include <winternl.h>
#include <process.h>
#include <ntstatus.h>
#include <cstring>*/

#include "stdafx.h"
#include <process.h>
#include <winternl.h>
#include <atlstr.h>

#include <iostream>//TODO: remove
#include <fstream>
#include <cassert>

#if FALSE
//https://processhacker.sourceforge.io/doc/ntexapi_8h_source.html#l02356
/*NTSYSCALLAPI NTSTATUS NTAPI NtQuerySystemInformation(
	    _In_ SYSTEM_INFORMATION_CLASS SystemInformationClass,
	    _Out_writes_bytes_opt_(SystemInformationLength) PVOID SystemInformation,
	    _In_ ULONG SystemInformationLength,
	    _Out_opt_ PULONG ReturnLength
	);*/

typedef struct _SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX
{
	    PVOID Object;
	    ULONG_PTR UniqueProcessId;
	    ULONG_PTR HandleValue;
	    ULONG GrantedAccess;
	    USHORT CreatorBackTraceIndex;
	    USHORT ObjectTypeIndex;
	    ULONG HandleAttributes;
	    ULONG Reserved;
	} SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX, *PSYSTEM_HANDLE_TABLE_ENTRY_INFO_EX;

typedef struct _SYSTEM_HANDLE_INFORMATION_EX
{
	    ULONG_PTR NumberOfHandles;
	    ULONG_PTR Reserved;
	    SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX Handles[1];
	} SYSTEM_HANDLE_INFORMATION_EX, *PSYSTEM_HANDLE_INFORMATION_EX;

//https://stackoverflow.com/questions/47101484/winapi-close-mutex-without-being-the-owner
void CloseRemote(ULONG dwProcessId, PCWSTR Name)
{
	// create any file
	HANDLE hFile = OpenMutex(MAXIMUM_ALLOWED, FALSE, Name);

	if (hFile != INVALID_HANDLE_VALUE)
	{
		if (HANDLE hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, dwProcessId))
		{

			NTSTATUS status;
			ULONG cb = 0x80000;
			
			union {
				PSYSTEM_HANDLE_INFORMATION_EX pshi;
				PVOID pv;
			};

			do
			{
				status = STATUS_INSUFFICIENT_RESOURCES;

				if (pv = LocalAlloc(0, cb))
				{
					//if (0 <= (status = NtQuerySystemInformation(SystemExtendedHandleInformation, pv, cb, &cb)))
					if (0 <= (status = NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)16, pv, cb, &cb)))
					{
						if (ULONG_PTR NumberOfHandles = pshi->NumberOfHandles)
						{
							ULONG_PTR UniqueProcessId = GetCurrentProcessId();
							//ULONG_PTR UniqueProcessId = dwProcessId;
							PSYSTEM_HANDLE_TABLE_ENTRY_INFO_EX Handles = pshi->Handles;
							do
							{
								// search for created file
								//if (Handles->UniqueProcessId == UniqueProcessId) {}
								//if (Handles->HandleValue == (ULONG_PTR)hFile) {}
								//if (Handles->UniqueProcessId == UniqueProcessId && Handles->HandleValue == (ULONG_PTR)hFile) {}
								if (Handles->UniqueProcessId == UniqueProcessId && Handles->HandleValue == (ULONG_PTR)hFile)
								{
									// we got it !
									//PVOID Object = Handles->Object;

									//NumberOfHandles = pshi->NumberOfHandles, Handles = pshi->Handles;
									/*do
									{
										if (Object == Handles->Object && Handles->UniqueProcessId == dwProcessId)
										{
											DuplicateHandle(hProcess, (HANDLE)Handles->HandleValue, 0, 0, 0, 0, DUPLICATE_CLOSE_SOURCE);
										}

									} while (Handles++, --NumberOfHandles);*/

									break;//CRASHES IT
								}
							} while (Handles++, --NumberOfHandles);
						}
					}
					LocalFree(pv);
				}

			} while (status == STATUS_INFO_LENGTH_MISMATCH);

			CloseHandle(hProcess);
		}

		CloseHandle(hFile);
	}
}
#endif

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

/*enum OBJECT_INFORMATION_CLASS
{
	ObjectBasicInformation,
	ObjectNameInformation,
	ObjectTypeInformation,
	ObjectAllInformation,
	ObjectDataInformation
};*/

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

//TODO: use https://docs.microsoft.com/en-gb/windows/desktop/api/fileapi/nf-fileapi-getfinalpathnamebyhandlea ??
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

	// 1 or 2???
	NtQueryObject()(h_File, xx, u8_Buffer, sizeof(u8_Buffer), &u32_ReqLength);//ObjectNameInformation not 2

	std::ofstream logging;
	logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
	logging << "Length=" << pk_Info->Length << ", MaxLength=" << pk_Info->MaximumLength << "\n";

	//WORD *p = (WORD*)(pk_Info->Buffer);
	logging << u8_Buffer[0] << "," << u8_Buffer[1] << "," << u8_Buffer[2] << "," << u8_Buffer[3] << "," << u8_Buffer[4] << "," << u8_Buffer[5] << "," << u8_Buffer[6] << "\n";

	logging.close();

	// On error pk_Info->Buffer is NULL
	if (!pk_Info->Buffer || !pk_Info->Length)
		return ERROR_FILE_NOT_FOUND;

	pk_Info->Buffer[pk_Info->Length / 2] = 0; // Length in Bytes!

	*ps_NTPath = pk_Info->Buffer;
	return 0;
}


/*namespace test
{

	enum OBJECT_INFORMATION_CLASS { ObjectNameInformation = 1 };
	enum FILE_INFORMATION_CLASS { FileNameInformation = 9 };
	struct FILE_NAME_INFORMATION { ULONG FileNameLength; WCHAR FileName[1]; };
	struct IO_STATUS_BLOCK { PVOID Dummy; ULONG_PTR Information; };
	struct UNICODE_STRING { USHORT Length; USHORT MaximumLength; PWSTR Buffer; };
	struct MOUNTMGR_TARGET_NAME { USHORT DeviceNameLength; WCHAR DeviceName[1]; };
	struct MOUNTMGR_VOLUME_PATHS { ULONG MultiSzLength; WCHAR MultiSz[1]; };

	extern "C" NTSYSAPI NTSTATUS NTAPI NtQueryObject(IN HANDLE Handle OPTIONAL,
		IN OBJECT_INFORMATION_CLASS ObjectInformationClass,
		OUT PVOID ObjectInformation OPTIONAL, IN ULONG ObjectInformationLength,
		OUT PULONG ReturnLength OPTIONAL);
	extern "C" NTSYSAPI NTSTATUS NTAPI NtQueryInformationFile(IN HANDLE FileHandle,
		OUT PIO_STATUS_BLOCK IoStatusBlock, OUT PVOID FileInformation,
		IN ULONG Length, IN FILE_INFORMATION_CLASS FileInformationClass);

#define MOUNTMGRCONTROLTYPE ((ULONG) 'm')
#define IOCTL_MOUNTMGR_QUERY_DOS_VOLUME_PATH  CTL_CODE(MOUNTMGRCONTROLTYPE, 12, METHOD_BUFFERED, FILE_ANY_ACCESS)

	union ANY_BUFFER {
		MOUNTMGR_TARGET_NAME TargetName;
		MOUNTMGR_VOLUME_PATHS TargetPaths;
		FILE_NAME_INFORMATION NameInfo;
		UNICODE_STRING UnicodeString;
		WCHAR Buffer[USHRT_MAX];
	};

	LPWSTR GetFilePath(HANDLE hFile)
	{
		static ANY_BUFFER nameFull, nameRel, nameMnt;
		ULONG returnedLength; IO_STATUS_BLOCK iosb; NTSTATUS status;
		status = NtQueryObject(hFile, ObjectNameInformation, nameFull.Buffer, sizeof(nameFull.Buffer), &returnedLength);
		assert(status == 0);
		status = NtQueryInformationFile(hFile, (PIO_STATUS_BLOCK)(&iosb), nameRel.Buffer,
			sizeof(nameRel.Buffer), FileNameInformation);
		assert(status == 0);
		//I'm not sure how this works with network paths...
		assert(nameFull.UnicodeString.Length >= nameRel.NameInfo.FileNameLength);
		nameMnt.TargetName.DeviceNameLength = (USHORT)(
			nameFull.UnicodeString.Length - nameRel.NameInfo.FileNameLength);
		wcsncpy(nameMnt.TargetName.DeviceName, nameFull.UnicodeString.Buffer,
			nameMnt.TargetName.DeviceNameLength / sizeof(WCHAR));
		HANDLE hMountPointMgr = CreateFile(_T("\\\\.\\MountPointManager"),
			0, FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
			NULL, OPEN_EXISTING, 0, NULL);
		__try
		{
			DWORD bytesReturned;
			BOOL success = DeviceIoControl(hMountPointMgr,
				IOCTL_MOUNTMGR_QUERY_DOS_VOLUME_PATH, &nameMnt,
				sizeof(nameMnt), &nameMnt, sizeof(nameMnt),
				&bytesReturned, NULL);
			assert(success && nameMnt.TargetPaths.MultiSzLength > 0);
			wcsncat(nameMnt.TargetPaths.MultiSz, nameRel.NameInfo.FileName,
				nameRel.NameInfo.FileNameLength / sizeof(WCHAR));
			return nameMnt.TargetPaths.MultiSz;
		}
		__finally { CloseHandle(hMountPointMgr); }
	}


}*/



typedef NTSTATUS(NTAPI* lpNtDuplicateObject)(HANDLE SourceProcessHandle, HANDLE SourceHandle, HANDLE TargetProcessHandle, PHANDLE TargetHandle, ACCESS_MASK DesiredAccess, ULONG Attributes, ULONG Options);
static lpNtDuplicateObject NtDuplicateObject = nullptr;



//https://www.codeguru.com/cpp/w-p/system/processesmodules/article.php/c2827/Examine-Information-on-Windows-NT-System-Level-Primitives.htm
int Close(DWORD m_processId)
{
	DWORD size = 0x2000;
	DWORD needed = 0;
	DWORD i = 0;
	BOOL  ret = 0;
	//CString strType;

	//m_HandleInfos.RemoveAll();

	//if (!INtDll::NtDllStatus)
	//	return FALSE;
	
	HMODULE hNtdll = LoadLibraryA("ntdll");
	NtDuplicateObject = (lpNtDuplicateObject)GetProcAddress(hNtdll, "NtDuplicateObject");


	

	// Allocate the memory for the buffer
	SYSTEM_HANDLE_INFORMATION* pSysHandleInformation = (SYSTEM_HANDLE_INFORMATION*)VirtualAlloc(NULL, size, MEM_COMMIT, PAGE_READWRITE);

	if (pSysHandleInformation == NULL)
		return FALSE;

	// Query the needed buffer size for the objects ( system wide )
	if (NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)16, pSysHandleInformation, size, &needed) != 0)
	{
		if (needed == 0)
		{
			ret = FALSE;
			goto cleanup;
		}

		// The size was not enough
		VirtualFree(pSysHandleInformation, 0, MEM_RELEASE);

		pSysHandleInformation = (SYSTEM_HANDLE_INFORMATION*)VirtualAlloc(NULL, size = needed + 256, MEM_COMMIT, PAGE_READWRITE);
	}

	if (pSysHandleInformation == NULL)
		return -1;

	// Query the objects ( system wide )
	if (NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)16, pSysHandleInformation, size, NULL) != 0)
	{
		ret = -1;
		goto cleanup;
	}

	
	
	

	// Iterating through the objects
	for (i = 0; i < pSysHandleInformation->Count; i++)
	{
		// ProcessId filtering check
		if (pSysHandleInformation->Handles[i].ProcessID == m_processId)
		{
			/*BOOL bAdd = FALSE;

			if (m_strTypeFilter == _T(""))
				bAdd = TRUE;
			else
			{
				// Type filtering
				GetTypeToken((HANDLE)pSysHandleInformation->Handles[i].HandleNumber, strType, pSysHandleInformation->Handles[i].ProcessID);

				bAdd = strType == m_strTypeFilter;
			}

			// That's it. We found one.
			if (bAdd)
			{*/
			HANDLE hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, m_processId);
			HANDLE handle = (HANDLE)(pSysHandleInformation->Handles[i].HandleNumber);

			HANDLE hHandleLocal = NULL;
			NTSTATUS status = NtDuplicateObject(hProcess, handle, GetCurrentProcess(), &hHandleLocal, 0, 0, DUPLICATE_SAME_ACCESS);// | DUPLICATE_SAME_ATTRIBUTES);

			CString path;

			/*DWORD _ret = GetHandleName(handle, &path, objectinfo::ObjectTypeInformation);
			if (_ret == 0)
			{

				std::ofstream logging;
				logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
				logging << "type=" << path << "\n";
				logging.close();


				if (path.Find("hl2_singleton_mutex") != -1)
				{
					pSysHandleInformation->Handles[i].HandleType = (WORD)(pSysHandleInformation->Handles[i].HandleType % 256);
					HANDLE hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, m_processId);
					DuplicateHandle(hProcess, handle, 0, 0, 0, 0, DUPLICATE_CLOSE_SOURCE);
				}
			}
			else

			{
				std::ofstream logging;
				logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
				logging << "error = " << _ret << "\n";
				logging.close();
			}*/

			DWORD _ret = GetHandleName(hHandleLocal, &path, objectinfo::ObjectNameInformation);
			CloseHandle(hHandleLocal);
			if (_ret == 0)
			{

				std::ofstream logging;
				logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
				logging << "name="<<path << "\n";
				logging.close();


				if (path.Find("hl2_singleton_mutex") != -1)
				{
					pSysHandleInformation->Handles[i].HandleType = (WORD)(pSysHandleInformation->Handles[i].HandleType % 256);
					
					//NtDuplicateObject(hProcess, handle, GetCurrentProcess(), &handle, 0, 0, DUPLICATE_CLOSE_SOURCE);

					std::ofstream logging;
					logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
					logging << "HL2HANDLE=" << handle << "\n";
					logging.close();
					

					/*{
						HANDLE dummyHandle = NULL;
						BOOL err_ret = CloseHandle(handle);
						if (err_ret == FALSE)
						{
							//TODO: http://forum.madshi.net/viewtopic.php?t=217
							std::ofstream logging;
							logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
							logging << "CLOSE_ERROR=" << GetLastError() << "\n";
							logging.close();
						}
						
					}*/

					{
						HANDLE dummyHandle = NULL;
						BOOL err_ret = DuplicateHandle(hProcess, handle, GetCurrentProcess(), &dummyHandle, 0, FALSE, DUPLICATE_CLOSE_SOURCE | DUPLICATE_SAME_ACCESS);
						if (err_ret == FALSE)
						{
							std::ofstream logging;
							logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
							logging << "DUPLICATE_ERROR=" << GetLastError() << "\n";
							logging.close();
						}
						CloseHandle(dummyHandle);
					}
				}
			}
			else

			{
				std::ofstream logging;
				logging.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\HooksCPP_Output.txt", std::ios_base::app);
				logging << "error = " << _ret << "\n";
				logging.close();
			}

			//m_HandleInfos.AddTail(pSysHandleInformation->Handles[i]);

			//}
		}
	}

	

cleanup:

	if (pSysHandleInformation != NULL)
		VirtualFree(pSysHandleInformation, 0, MEM_RELEASE);

	return ret;
}

extern "C" __declspec(dllexport) int SourceEngineUnlock(int pid)
{
	//CloseRemote((ULONG)pid, L"hl2_singleton_mutex");
	//return 1;//TODO: set to 0

	return Close(pid);
}