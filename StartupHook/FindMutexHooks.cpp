#include "stdafx.h"
#include "InstallHooks.h"
#include "Hooking.h"
#include <vector>
#include <ntstatus.h>
#include <winternl.h>
#include <random>
#include <map>

std::mt19937 randomGenerator;

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

inline UNICODE_STRING stdWStringToUnicodeString(const std::wstring& str) {
	UNICODE_STRING unicodeString;
	DWORD len = 0;

	len = str.length();
	LPWSTR cstr = new WCHAR[len + 1];
	memcpy(cstr, str.c_str(), (len + 1) * sizeof(WCHAR));
	unicodeString.Buffer = cstr;
	unicodeString.Length = (USHORT)(len * sizeof(WCHAR));
	unicodeString.MaximumLength = (USHORT)((len + 1) * sizeof(WCHAR));
	return unicodeString;
}

void updateName(PUNICODE_STRING inputName)
{
	if (!(inputName->Length > 0 && inputName->Length <= inputName->MaximumLength)) return;

	for (std::map<std::wstring, std::wstring>::value_type& pair : searchTermsToAssignedNames)
	{
		if (wcsstr(inputName->Buffer, pair.first.c_str()) != nullptr)
		{
			if (pair.second.empty())
			{
				const auto rand = std::to_wstring(randomGenerator());

				const std::wstring oldName = inputName->Buffer;
				const auto newName = oldName + rand;

				pair.second = newName;
				MessageBoxW(NULL, newName.c_str(), oldName.c_str(), 0);
			}

			*inputName = stdWStringToUnicodeString(pair.second);
		}
	}
}

inline void updateNameObject(POBJECT_ATTRIBUTES ObjectAttributes)
{
	if (ObjectAttributes != NULL && ObjectAttributes->ObjectName != NULL)
	{
		updateName(ObjectAttributes->ObjectName);
	}
}

NTSTATUS NTAPI NtCreateMutant_Hook(OUT PHANDLE MutantHandle, IN ULONG DesiredAccess, IN POBJECT_ATTRIBUTES ObjectAttributes OPTIONAL, IN BOOLEAN InitialOwner)
{
	updateNameObject(ObjectAttributes);
	return NtCreateMutant(MutantHandle, DesiredAccess, ObjectAttributes, InitialOwner);
}

NTSTATUS NTAPI NtOpenMutant_Hook(PHANDLE MutantHandle, ULONG DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes)
{
	updateNameObject(ObjectAttributes);
	return NtOpenMutant(MutantHandle, DesiredAccess, ObjectAttributes);
}

NTSTATUS NTAPI NtCreateEvent_Hook(PHANDLE EventHandle, DWORD DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes, EVENT_TYPE EventType, BOOLEAN InitialState)
{
	updateNameObject(ObjectAttributes);
	return NtCreateEvent(EventHandle, DesiredAccess, ObjectAttributes, EventType, InitialState);
}

void installFindMutexHooks()
{
	//Random
	std::random_device rd;
	randomGenerator = static_cast<std::mt19937>(rd());

	//Search terms
#define ADD_SEARCH_TERM(term) searchTermsToAssignedNames.insert(std::make_pair((term), L""));
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
}