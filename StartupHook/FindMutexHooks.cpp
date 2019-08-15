#include "stdafx.h"
#include "InstallHooks.h"
#include "Hooking.h"
#include <vector>
#include <ntstatus.h>
#include <winternl.h>
#include <random>
#include <map>

std::mt19937 randomGenerator;

//The state of a mutex object is signaled when it is not owned by any thread
std::vector<HANDLE> handlesToFakeSignal;

//Key: search term. Value: the assigned name that is replaced for every name that matched the search term. (value is empty if needs generating)
std::map <std::wstring, std::wstring> searchTermsToAssignedNames;

typedef enum _EVENT_TYPE {
	NotificationEvent,
	SynchronizationEvent
} EVENT_TYPE, *PEVENT_TYPE;

typedef NTSTATUS(NTAPI* t_NtCreateMutant)(PHANDLE MutantHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, BOOLEAN InitialOwner);
typedef NTSTATUS(NTAPI* t_NtOpenMutant)(PHANDLE MutantHandle, ULONG DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes);
typedef NTSTATUS(NTAPI* t_NtCreateEvent)(PHANDLE EventHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, EVENT_TYPE EventType, BOOLEAN InitialState);

static t_NtCreateMutant NtCreateMutant;

//Returns STATUS_OBJECT_NAME_NOT_FOUND or STATUS_SUCCESS (or STATUS_OBJECT_NAME_COLLISION if OBJ_OPENIF is specified in OBJECT_ATTRIBUTES)
static t_NtOpenMutant NtOpenMutant;

static t_NtCreateEvent NtCreateEvent;

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

UNICODE_STRING stdWStringToUnicodeString(const std::wstring& str) {
	UNICODE_STRING lsaWStr;
	DWORD len = 0;

	len = str.length();
	LPWSTR cstr = new WCHAR[len + 1];
	memcpy(cstr, str.c_str(), (len + 1) * sizeof(WCHAR));
	lsaWStr.Buffer = cstr;
	lsaWStr.Length = (USHORT)((len) * sizeof(WCHAR));
	lsaWStr.MaximumLength = (USHORT)((len + 1) * sizeof(WCHAR));
	return lsaWStr;
}

void updateName(PUNICODE_STRING inputName)
{
	if (!(inputName->Length > 0 && inputName->Length <= inputName->MaximumLength)) return;

	for (auto& pair : searchTermsToAssignedNames)
	{
		if (wcsstr(inputName->Buffer, pair.first.c_str()) != nullptr)
		{
			if (pair.second.empty())
			{
				const auto rand = std::to_wstring(randomGenerator());

				const std::wstring oldName = inputName->Buffer;
				const auto newName = oldName + rand;

				pair.second = newName;
			}

			*inputName = stdWStringToUnicodeString(pair.second);
		}
	}
}

NTSTATUS NTAPI NtCreateMutant_Hook(OUT PHANDLE MutantHandle, IN ULONG DesiredAccess, IN POBJECT_ATTRIBUTES ObjectAttributes OPTIONAL, IN BOOLEAN InitialOwner)
{
	const NTSTATUS ret = NtCreateMutant(MutantHandle, DesiredAccess, ObjectAttributes, InitialOwner);

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

NTSTATUS NTAPI NtOpenMutant_Hook(PHANDLE MutantHandle, ULONG DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes)
{
	const auto ret = NtOpenMutant(MutantHandle, DesiredAccess, ObjectAttributes);

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

NTSTATUS NTAPI NtCreateEvent_Hook(PHANDLE EventHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, EVENT_TYPE EventType, BOOLEAN InitialState)
{
	if (ObjectAttributes != nullptr && ObjectAttributes->ObjectName != nullptr && isTargetName(*(ObjectAttributes->ObjectName)))
	{
		auto rand = std::to_wstring(randomGenerator());
		MessageBoxW(NULL, rand.c_str(), L"rand", 0);
		std::wstring name0 = ObjectAttributes->ObjectName->Buffer;
		std::wstring name = name0 + rand;
		//auto nameWC = const_cast<wchar_t*>(name.c_str());
		auto nameWC = name.c_str();

		MessageBoxW(NULL, nameWC, L"nameWC", 0);

		auto s = stdWStringToUnicodeString(name);
		ObjectAttributes->ObjectName = &s;
	}

	return NtCreateEvent(EventHandle, DesiredAccess, ObjectAttributes, EventType, InitialState);
}

void installFindMutexHooks()
{
	//Random
	std::random_device rd;
	randomGenerator = static_cast<std::mt19937>(rd());

	//Search terms
#define ADD_SEARCH_TERM(term) searchTermsToAssignedNames.insert((term), L"")
	ADD_SEARCH_TERM(L"Overkill Engine Game");
	ADD_SEARCH_TERM(L"hl2_singleton_mutex");
#undef ADD_SEARCH_TERM

	//Ntdll functions
#define GET_NT_PROC(name, type) (type)GetProcAddress(GetModuleHandle("ntdll.dll"), name)
	NtCreateMutant = GET_NT_PROC("NtCreateMutant", t_NtCreateMutant);
	NtOpenMutant = GET_NT_PROC("NtOpenMutant", t_NtOpenMutant);
	NtCreateEvent = GET_NT_PROC("NtCreateEvent", t_NtCreateEvent);
#undef GET_NT_PROC

	//Hooks
	installHook("ntdll.dll", "NtCreateMutant", NtCreateMutant_Hook);
	installHook("ntdll.dll", "NtOpenMutant", NtOpenMutant_Hook);

	installHook("ntdll.dll", "NtCreateEvent", NtCreateEvent_Hook);

	installHook("kernel32.dll", "WaitForSingleObject", WaitForSingleObject_Hook);
}