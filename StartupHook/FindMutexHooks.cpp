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

typedef NTSTATUS(NTAPI* t_NtOpenMutant)(PHANDLE MutantHandle, ULONG DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes);

//Returns STATUS_OBJECT_NAME_NOT_FOUND or STATUS_SUCCESS (or STATUS_OBJECT_NAME_COLLISION if OBJ_OPENIF is specified in OBJECT_ATTRIBUTES)
t_NtOpenMutant NtOpenMutant()
{
	static t_NtOpenMutant f_NtOpenMutant = NULL;
	if (!f_NtOpenMutant)
	{
		HMODULE h_NtDll = GetModuleHandle("Ntdll.dll");
		f_NtOpenMutant = (t_NtOpenMutant)GetProcAddress(h_NtDll, "NtOpenMutant");
	}
	return f_NtOpenMutant;
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
	return isHandleToFakeSignal(hHandle) ? WAIT_OBJECT_0 : WaitForSingleObject(hHandle, dwMilliseconds);
}

DWORD WINAPI WaitForSingleObjectEx_Hook(
	HANDLE hHandle,
	DWORD  dwMilliseconds,
	BOOL   bAlertable
)
{
	return isHandleToFakeSignal(hHandle) ? WAIT_OBJECT_0 : WaitForSingleObjectEx(hHandle, dwMilliseconds, bAlertable);
}

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

NTSTATUS WINAPI NtCreateMutant_Hook(OUT PHANDLE MutantHandle, IN ULONG DesiredAccess, IN POBJECT_ATTRIBUTES ObjectAttributes OPTIONAL, IN BOOLEAN InitialOwner)
{
	const NTSTATUS ret = NtCreateMutant()(MutantHandle, DesiredAccess, ObjectAttributes, InitialOwner);

	if (ObjectAttributes != nullptr && ObjectAttributes->ObjectName != nullptr && isTargetName(*(ObjectAttributes->ObjectName)))
	{
		if (InitialOwner == FALSE)
		{
			handlesToFakeSignal.push_back(*MutantHandle);
		}

		SetLastError(ERROR_SUCCESS);
		return STATUS_SUCCESS;
	}

	return ret;
}

NTSTATUS WINAPI NtOpenMutant_Hook(PHANDLE MutantHandle, ULONG DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes)
{
	const auto ret = NtOpenMutant()(MutantHandle, DesiredAccess, ObjectAttributes);

	if (ObjectAttributes == nullptr || ObjectAttributes->ObjectName == nullptr || !isTargetName(*ObjectAttributes->ObjectName))
	{
		return ret;
	}

	if (ret == STATUS_OBJECT_NAME_COLLISION)
	{
		SetLastError(ERROR_SUCCESS);
		return STATUS_SUCCESS;
	}
	else if (ret == STATUS_SUCCESS)
	{
		*MutantHandle = NULL;
		SetLastError(ERROR_OBJECT_NOT_FOUND);
		return STATUS_OBJECT_NAME_NOT_FOUND;
	}

	return ret;
}

void installFindMutexHooks()
{
	installHook("ntdll.dll", "NtCreateMutant", NtCreateMutant_Hook);
	installHook("ntdll.dll", "NtOpenMutant", NtOpenMutant_Hook);
	installHook("kernel32.dll", "WaitForSingleObject", WaitForSingleObject_Hook);
}