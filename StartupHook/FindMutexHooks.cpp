#include "stdafx.h"
#include "InstallHooks.h"
#include "Hooking.h"
#include <vector>
#include <ntstatus.h>

//The state of a mutex object is signaled when it is not owned by any thread
std::vector<HANDLE> handlesToFakeSignal;

typedef NTSTATUS(NTAPI* t_NtCreateMutant)(PHANDLE MutantHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, BOOLEAN InitialOwner);

t_NtCreateMutant NtCreateMutant()
{
	static t_NtCreateMutant f_NtCreateMutant = NULL;
	if (!f_NtCreateMutant)
	{
		HMODULE h_NtDll = GetModuleHandle("Ntdll.dll");
		f_NtCreateMutant = (t_NtCreateMutant)GetProcAddress(h_NtDll, "NtCreateMutant");
	}
	return f_NtCreateMutant;
}

bool isHandleToFakeSignal(HANDLE handle)
{
	return handlesToFakeSignal.empty() ? false : 
		std::find(handlesToFakeSignal.begin(), handlesToFakeSignal.end(), handle) != handlesToFakeSignal.end();
}

DWORD WINAPI WaitForSingleObject_Hook(
	HANDLE hHandle,
	DWORD  dwMilliseconds
)
{
	if (isHandleToFakeSignal(hHandle))
	{
		//MessageBox(NULL, "A", "1", MB_OK);
		return WAIT_OBJECT_0;//Already signaled
	}

	auto ret = WaitForSingleObject(hHandle, dwMilliseconds);
	return ret == WAIT_FAILED ? WAIT_OBJECT_0 : ret;
}

/*HANDLE WINAPI CreateMutexA_Hook(
	LPSECURITY_ATTRIBUTES lpMutexAttributes,
	BOOL                  bInitialOwner,
	LPCSTR                lpName
)
{
	if (isTargetName(lpName))
	{
		MessageBox(NULL, lpName, "0A", MB_OK);
		HANDLE handle = CreateMutexA(lpMutexAttributes, bInitialOwner, lpName);

		//MessageBox(NULL, "B", "0", MB_OK);
		//If bInitialOwner is TRUE, the mutex is set to NOT signaled. (It is signaled if NO thread owns it).
		//Hence there is no point returning WAIT_OBJECT_0 in WaitForSignalObject (ie already signaled)
		if (bInitialOwner == FALSE)
		{
			//MessageBox(NULL, "C", "0", MB_OK);
			handlesToFakeSignal.push_back(handle);
			//MessageBox(NULL, "D", "0", MB_OK);
		}
		//MessageBox(NULL, "E", "0", MB_OK);

		//CreateMutex sets this to ERROR_ALREADY_EXISTS
		SetLastError(ERROR_SUCCESS);
		//MessageBox(NULL, "F", "0", MB_OK);
		MessageBox(NULL, ("handle = " + std::to_string((int)handle)).c_str(), "handle", 0);
		//return NULL;
		return handle;
	}
	else
	{
		return CreateMutexA(lpMutexAttributes, bInitialOwner, lpName);
	}
}*/

bool isTargetName(UNICODE_STRING name)
{
	if (name.Length > 0 && name.Length <= name.MaximumLength)
	{
		std::wstring hl2 = L"hl2_singleton_mutex";
		return wcsstr(name.Buffer, hl2.c_str()) != nullptr;

		/*std::wstring wStrBuf(name.Buffer, name.Length / sizeof(WCHAR));
		const wchar_t *wStr = wStrBuf.c_str();
		if (name.Length == wStrBuf.length())
		{
			return wcsstr(wStr, hl2.c_str()) != nullptr;
		}*/
	}

	return false;
}

NTSTATUS WINAPI NtCreateMutant_Hook(PHANDLE MutantHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, BOOLEAN InitialOwner)
{
	const NTSTATUS ret = NtCreateMutant()(MutantHandle, DesiredAccess, ObjectAttributes, InitialOwner);

	if (ObjectAttributes != nullptr && ObjectAttributes->ObjectName != nullptr && isTargetName(*(ObjectAttributes->ObjectName)))
	//if (false)
	{
		MessageBox(NULL, "A", "00A", MB_OK);

		if (InitialOwner == FALSE)
		{
			handlesToFakeSignal.push_back(*MutantHandle);
		}

		SetLastError(ERROR_SUCCESS);
		return STATUS_SUCCESS;
	}

	return ret;
}

void installFindMutexHooks()
{
	installHook("ntdll.dll", "NtCreateMutant", NtCreateMutant_Hook);
	//installHook("kernel32.dll", "CreateMutexA", CreateMutexA_Hook);
	installHook("kernel32.dll", "WaitForSingleObject", WaitForSingleObject_Hook);
}