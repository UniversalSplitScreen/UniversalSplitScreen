#include "stdafx.h"
#include "Hooking.h"
#include <iostream>
#include <Windows.h>

NTSTATUS installHook(void* entryPoint, void* inCallback, std::string name)
{
	HOOK_TRACE_INFO hHook = { NULL };

	const NTSTATUS hookResult = LhInstallHook(
		entryPoint,
		inCallback,
		nullptr,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully installed hook '" << name << "'\n";
	}
	else
	{
		const auto msg = "Failed to install hook '" + name + "', NTSTATUS: " + std::to_string(hookResult);
		std::cerr << msg << std::endl;
		MessageBox(nullptr, msg.c_str(), "Error", MB_OK);
	}

	return hookResult;
}

NTSTATUS installHook(const LPCSTR moduleHandle, std::string lpProcName, void* inCallback)
{
	HOOK_TRACE_INFO hHook = { NULL };

	const NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(moduleHandle), lpProcName.c_str()),
		inCallback,
		nullptr,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully installed hook " << lpProcName << " in module '" << moduleHandle << "'\n";
	}
	else
	{
		const std::string msg = "Failed to install hook " + lpProcName + " in module '" + moduleHandle + "', NTSTATUS: " + std::to_string(hookResult);
		std::cerr << msg << std::endl;
		MessageBox(nullptr, msg.c_str(), "Error", MB_OK);
	}

	return hookResult;
}