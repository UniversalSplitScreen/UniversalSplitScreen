#include "stdafx.h"
#include "DirectInputHook.h"
#include "Hooking.h"
#include <dinput.h>
#include <iostream>

#ifdef X64
using PtrSize = long long;//64 bit pointers
#else
using PtrSize = int; //32 bit pointers
#endif

extern HMODULE DllHandle;

LPDIRECTINPUT8 pDinput8;
LPDIRECTINPUT7 pDinput7;

static GUID controllerGuid = GUID_NULL;
static int controllerIndex = 0;

static BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, LPVOID pvRef)
{
	auto di = *lpddi;
	const auto i = static_cast<int*>(pvRef);
	
	/* https://www.usb.org/sites/default/files/documents/hut1_12v2.pdf page 26:
		4 : Joystick
		5 : Game Pad */
	if (di.wUsage == 4 || di.wUsage == 5)
	{
		*i += 1;
		if (*i == controllerIndex)//controllerIndex starts at 1
		{
			controllerGuid = di.guidInstance;
			std::cout << "Selected controller, instanceName=" << di.tszInstanceName << ", productName=" << di.tszProductName << ", usage=" << di.wUsage << 
				", usagePage=" << di.wUsagePage << ", *i=" << *i << "\n";
			return DIENUM_STOP;
		}
	}	

	return DIENUM_CONTINUE;
}

void* GetDinput8CreateDevicePtr()
{
	const auto vPtrDinput8 = *reinterpret_cast<PtrSize**>(pDinput8);
	return reinterpret_cast<void*>(vPtrDinput8[3]);
}

bool GetDinput7CreateDevicePtr(void** pPtr)
{
	LoadLibrary("dinput.dll");
	const auto directInputCreateEx = GetProcAddress(GetModuleHandle("dinput.dll"), "DirectInputCreateEx");
	typedef HRESULT(__stdcall * FuncDirectInputCreateEx)(HINSTANCE hinst, DWORD dwVersion, REFIID riidltf, LPVOID* ppvOut, LPUNKNOWN punkOuter);
	static auto DirectInputCreateEx = FuncDirectInputCreateEx(directInputCreateEx);

	if (DI_OK != DirectInputCreateEx(DllHandle, 0x0700, IID_IDirectInput7, reinterpret_cast<void**>(&pDinput7), nullptr))
		return false;
	
	const auto vPtrDinput7 = *reinterpret_cast<PtrSize**>(pDinput7);
	*pPtr = reinterpret_cast<void*>(vPtrDinput7[3]);

	return true;
}

HRESULT __stdcall Dinput8_CreateDevice_Hook(IDirectInput8* pDin, REFGUID rguid, LPDIRECTINPUTDEVICE8A* lplpDirectInputDevice, LPUNKNOWN pUnkOuter)
{
	std::cout << "Dinput8 CreateDeviceHook called\n";

	if (controllerIndex == 0)
	{
		return DIERR_INVALIDPARAM; //pretend it failed
	}

	return pDin->CreateDevice(controllerGuid, lplpDirectInputDevice, pUnkOuter);
}

HRESULT __stdcall Dinput7_CreateDevice_Hook(IDirectInput7* pDin, REFGUID rguid, LPDIRECTINPUTDEVICEA* lplpDirectInputDevice, LPUNKNOWN pUnkOuter)
{
	std::cout << "Dinput7 CreateDeviceHook called\n";

	if (controllerIndex == 0)
	{
		return DIERR_INVALIDPARAM; //pretend it failed
	}

	return pDin->CreateDevice(controllerGuid, lplpDirectInputDevice, pUnkOuter);
}

void installDirectInputHooks(int _controllerIndex)
{
	controllerIndex = _controllerIndex;

	if (DI_OK != DirectInput8Create(DllHandle, DIRECTINPUT_VERSION, IID_IDirectInput8, reinterpret_cast<void**>(&pDinput8), nullptr))
	{
		MessageBox(nullptr, "Error creating dinput8 interface", "DirectInput8 error", MB_OK);
		std::cerr << "Error creating dinput8 interface" << std::endl;
		return;
	}

	if (controllerIndex != 0)
	{
		int i = 0;
		pDinput8->EnumDevices(DI8DEVCLASS_ALL, DIEnumDevicesCallback, &i, DIEDFL_ALLDEVICES);

		if (controllerGuid == GUID_NULL)
		{
			MessageBox(nullptr, "Not enough controllers", "DirectInput8 error", MB_OK);
			std::cerr << "Not enough controllers" << std::endl;
			return;
		}
	}

	installHook(GetDinput8CreateDevicePtr(), Dinput8_CreateDevice_Hook, "Dinput8 CreateDevice");

	void* pDinput7CreateDevice;
	if (!GetDinput7CreateDevicePtr(&pDinput7CreateDevice))
	{
		MessageBox(nullptr, "Error creating dinput7 interface", "DirectInput7 error", MB_OK);
		std::cerr << "Error creating dinput7 interface" << std::endl;
	}

	installHook(pDinput7CreateDevice, Dinput7_CreateDevice_Hook, "Dinput7 CreateDevice");
}
