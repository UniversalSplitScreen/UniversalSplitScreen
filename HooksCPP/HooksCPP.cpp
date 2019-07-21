#include "stdafx.h"
#include <easyhook.h>
#include <string>
#include <iostream>
#include <Xinput.h>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <fstream>
#include <thread>
#include <time.h>
//#include <Xinput.h>
#include <windowsx.h>
#include <dinput.h>
using namespace std;

#if _WIN64
#define X64
#else
#define X86
#endif

extern HMODULE DllHandle;

HWND hWnd = 0;
string _ipcChannelNameRead;//The name of the named pipe.
string _ipcChannelNameWrite;

HANDLE hPipeRead;
HANDLE hPipeWrite;

CRITICAL_SECTION mcs;
int fakeX;//Delta X
int fakeY;

int absoluteX;
int absoluteY;

bool updateAbsoluteFlagInMouseMessage;

// If enabled, will alternate between absolute mouse position (bound to 0,0 and width,height) and delta mouse position (returns the changes, without bounding)
bool enableLegacyInput = true; 

BOOL useAbsoluteCursorPos = TRUE;
int useAbsoluteCursorPosCounter = 0;// 0/1/2/3/... : FALSE, requiredAbsCount : TRUE
const int requiredAbsCount = 40;//Requires higher number for higher mouse polling rate

//time_t timeSinceLastSetCursorPos;
//Time since the last SetCursorPos that we will assume the game is in a UI menu and needs absolute mouse position
//const double minTimeForAbs = 0;


UINT16 vkey_state;//Stores the mouse keys (5 of them) and the WASD keys. (1=on, 0=off)
int controllerIndex = 0;//The controller index for this game.
HANDLE allowedMouseHandle = 0;//We will allow raw input from this mouse handle.

bool filterRawInput;
bool filterMouseMessages;

bool pipeClosed = false;

IDirectInput8* pDinput;

const int maxDinputDevices = 16;
GUID dinputGuids[maxDinputDevices];
GUID controllerGuid;
int dinputGuids_i = 0;

/*class dinput_deleter {
	
public:
	dinput_deleter()
	{
		pDinput = 0;
	}
	
	~dinput_deleter()
	{
		if (pDinput != 0)
		{
			//TODO: crashes game?
			//TODO: move to pipe end?
			pDinput->Release();
			pDinput = 0;
		}
	}
  };

dinput_deleter pDinput_deleter;*/

void UpdateAbsoluteCursorCheck()
{
	if (enableLegacyInput && useAbsoluteCursorPos == FALSE)
	{
		//double dt = difftime(time(NULL), timeSinceLastSetCursorPos);
		//if (dt >= minTimeForAbs)
		//{

		//We assume we're in a menu and need absolute cursor pos
		useAbsoluteCursorPosCounter++;
		if (useAbsoluteCursorPosCounter == requiredAbsCount)
		{
			useAbsoluteCursorPos = TRUE;
		}

		//}
	}
}

BOOL WINAPI GetCursorPos_Hook(LPPOINT lpPoint)
{
	if (lpPoint)
	{
		EnterCriticalSection(&mcs);
		if ((!enableLegacyInput) || useAbsoluteCursorPos == TRUE)
		{
			//Absolute mouse position (always do this if legacy input is off)
			lpPoint->x = absoluteX;
			lpPoint->y = absoluteY;
		}
		else
		{
			//Delta mouse position
			lpPoint->x = fakeX;
			lpPoint->y = fakeY;
		}

		LeaveCriticalSection(&mcs);
		ClientToScreen(hWnd, lpPoint);

		UpdateAbsoluteCursorCheck();
	}
	return true;
}

int originX, originY;

BOOL WINAPI SetCursorPos_Hook(int X, int Y)
{
	POINT p;
	p.x = X;
	p.y = Y;
	
	//SetCursorPos require screen coordinates (relative to 0,0 of monitor)
	ScreenToClient(hWnd, &p);

	originX = p.x;
	originY = p.y;
		
	if (!enableLegacyInput)
	{
		EnterCriticalSection(&mcs);
		absoluteX = p.x;
		absoluteY = p.y;
		LeaveCriticalSection(&mcs);
	}
	else
	{
		EnterCriticalSection(&mcs);
		fakeX = p.x;
		fakeY = p.y;
		LeaveCriticalSection(&mcs);

		//time(&timeSinceLastSetCursorPos);
		useAbsoluteCursorPosCounter = 0;
		useAbsoluteCursorPos = FALSE;
	}

	return TRUE;
}

HWND WINAPI GetForegroundWindow_Hook()
{
	return hWnd;
}

HWND WINAPI WindowFromPoint_Hook()
{
	return hWnd;
}

HWND WINAPI GetActiveWindow_Hook()
{
	return hWnd;
}

BOOL WINAPI IsWindowEnabled_Hook(HWND hWnd)
{
	return TRUE;
}

inline int getBitShiftForVKey(int VKey)
{
	int shift = 0;
	if (VKey <= 6)//The mouse keys
	{
		return VKey - 1;
	}
	else
	{
		//WASD keys
		switch (VKey)
		{
			case 0x41: return 6;
			case 0x44: return 7;
			case 0x53: return 8;
			case 0x57: return 9;
			default: return 10;
		}
	}
}

SHORT WINAPI GetAsyncKeyState_Hook(int vKey)
{
	return (vkey_state & (1 << getBitShiftForVKey(vKey))) == 0 ? // is the vKey up?
		0 : 0b1000000000000000;
}

SHORT WINAPI GetKeyState_Hook(int nVirtKey)
{
	if (nVirtKey == 0x41 || nVirtKey == 0x44 || nVirtKey == 0x53 || nVirtKey == 0x57)//WASD
	{
		return (vkey_state & (1 << getBitShiftForVKey(nVirtKey))) == 0 ? // is the vKey down?
			0 : 0b1000000000000000;
	}
	else
	{
		return GetKeyState(nVirtKey);
	}
}

BOOL WINAPI RegisterRawInputDevices_Hook(PCRAWINPUTDEVICE pRawInputDevices, UINT uiNumDevices, UINT cbSize)
{
	//Don't actually let raw input be registered, but pretend to the game it has
	return true;
}

DWORD WINAPI XInputGetState_Hook(DWORD dwUserIndex, XINPUT_STATE *pState)
{
	if (controllerIndex == 0) // user wants no controller on this game
		return ERROR_DEVICE_NOT_CONNECTED;
	else
		return XInputGetState(controllerIndex - 1, pState);
}

DWORD WINAPI XInputSetState_Hook(DWORD dwUserIndex, XINPUT_VIBRATION *pVibration)
{
	if (controllerIndex == 0)
		return ERROR_DEVICE_NOT_CONNECTED;
	else
		return XInputSetState(controllerIndex - 1, pVibration);
}

inline int bytesToInt(BYTE* bytes)
{
	return (int)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
}

void startPipeListen()
{	
	//Read pipe
	char _pipeNameChars[256];
	sprintf_s(_pipeNameChars, "\\\\.\\pipe\\%s", _ipcChannelNameRead.c_str());

	hPipeRead = CreateFile(
		_pipeNameChars,
		GENERIC_READ,
		FILE_SHARE_READ | FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL
	);

	if (hPipeRead == INVALID_HANDLE_VALUE)
	{
		std::cout << "Failed to connect to pipe (read)\n";
		return;
	}

	std::cout << "Connected to pipe (read)\n";

	//Write pipe
	char _pipeNameCharsWrite[256];
	sprintf_s(_pipeNameCharsWrite, "\\\\.\\pipe\\%s", _ipcChannelNameWrite.c_str());

	hPipeWrite = CreateFile(
		_pipeNameCharsWrite,
		GENERIC_WRITE,
		FILE_SHARE_READ | FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL
	);

	if (hPipeWrite == INVALID_HANDLE_VALUE)
	{
		std::cout << "Failed to connect to pipe (write)\n";
	}
	else
	{
		std::cout << "Connected to pipe (write)\n";
	}

	//Loop until pipe close message is received
	for (;;)
	{
		BYTE buffer[9];//9 bytes are sent at a time (1st is message, next 8 for 2 ints)
		DWORD bytesRead = 0;

		BOOL result = ReadFile(
			hPipeRead,
			buffer,
			9 * sizeof(BYTE),
			&bytesRead,
			NULL
		);

		if (result && bytesRead == 9)
		{
			int param1 = bytesToInt(&buffer[1]);

			int param2 = bytesToInt(&buffer[5]);

			//cout << "Received message. Msg=" << (int)buffer[0] << ", param1=" << param1 << ", param2=" << param2 << "\n";

			switch (buffer[0])
			{
				case 0x01://Add delta cursor pos
				{
					EnterCriticalSection(&mcs);
					fakeX += param1;
					fakeY += param2;
					LeaveCriticalSection(&mcs);
					break;
				}
				case 0x04://Set absolute cursor pos
				{
					EnterCriticalSection(&mcs);
					absoluteX = param1;
					absoluteY = param2;
					LeaveCriticalSection(&mcs);
					break;
				}
				case 0x02://Set VKey
				{
					UINT16 shift = (1 << getBitShiftForVKey(param1));
					if (param2 == 0)//Button up
					{
						vkey_state &= (~shift);//Sets to 0
					}
					else//Button down
					{
						vkey_state |= shift;//Sets to 1
					}
					break;
				}
				case 0x03://Close named pipe
				{
					std::cout << "Received pipe closed message. Closing pipe..." << "\n";
					pipeClosed = true;
					return;
				}
				case 0x05://Focus desktop
				{
					//If the game brings itself to the foreground, it is the only window that can set something else as foreground (so it's required to do this in HooksCPP)
					SetForegroundWindow(GetDesktopWindow());
					break;
				}
				default:
				{
					break;
				}
			}
		}
		else
		{
			//cout << "Failed to read message\n";
		}
	}
}

NTSTATUS installHook(LPCSTR moduleHandle, LPCSTR lpProcName, void* InCallback)
{
	HOOK_TRACE_INFO hHook = { NULL };

	NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(moduleHandle), lpProcName),
		InCallback,
		NULL,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully installed hook " << lpProcName << " in module '" << moduleHandle << "'\n";
	}
	else
	{
		std::cout << "Failed to install hook " << lpProcName << " in module '"<< moduleHandle << "', NTSTATUS: " << hookResult << "\n";
	}

	return hookResult;
}

int lastX, lastY;

bool sentVisibility = true; //the visibility stored in C#

void SetCursorVisibility(bool show)
{
	if (show != sentVisibility)
	{
		BYTE buffer[9] = { 0x06,  0,0,0, (show ? 1 : 0),  0,0,0,0 };

		DWORD bytesRead = 0;

		BOOL result = WriteFile(
			hPipeWrite,
			buffer,
			9 * sizeof(BYTE),
			&bytesRead,
			NULL
		);

		if (result == FALSE)
		{
			cout << "scv fail, err=" << GetLastError() << "\n";
		}

		sentVisibility = show;
	}

	//std::cout << "scvr, b="<<show << "\n";
}

int WINAPI ShowCursor_Hook(BOOL bShow)
{
	SetCursorVisibility(bShow == TRUE);
	if (bShow == FALSE) ShowCursor(FALSE);
	return (bShow == TRUE) ? 0 : -1;
	//return ShowCursor(bShow);
}

HCURSOR WINAPI SetCursor_Hook(HCURSOR hCursor)
{
	SetCursorVisibility(hCursor != NULL);
	if (hCursor == NULL) SetCursor(NULL);
	return hCursor;
	//return SetCursor(hCursor);
}

BOOL FilterMessage(LPMSG lpMsg)
{
	UINT Msg = lpMsg->message;
	WPARAM _wParam = lpMsg->wParam;
	LPARAM _lParam = lpMsg->lParam;

	//Filter raw input
	if (Msg == WM_INPUT && filterRawInput)
	{
		UINT dwSize = 0;
		const UINT sorh = sizeof(RAWINPUTHEADER);
		static RAWINPUT raw[sorh];

		if ((0 == GetRawInputData((HRAWINPUT)lpMsg->lParam, RID_HEADER, NULL, &dwSize, sorh)) && 
			(dwSize == sorh) &&
			(dwSize == GetRawInputData((HRAWINPUT)lpMsg->lParam, RID_HEADER, raw, &dwSize, sorh)) &&
			(raw->header.dwType == RIM_TYPEMOUSE))
		{
			if (raw->header.hDevice == allowedMouseHandle)
			{
				return 1;
			}
			else
			{
				memset(lpMsg, 0, sizeof(MSG));
				return -1;
			}
		}
	}

	//Legacy input filter
	if (enableLegacyInput)
	{
		if (Msg == WM_MOUSEMOVE)
		{
			if (((int)_wParam & 0b10000000) > 0)//Signature for message sent from USS (C#)
			{
				if (useAbsoluteCursorPos == FALSE)
				{
					if (updateAbsoluteFlagInMouseMessage)
					{
						int x = GET_X_LPARAM(_lParam);
						int y = GET_Y_LPARAM(_lParam);

						if (!(x == 0 && y == 0) && !(x == lastX && y == lastY))
							// - Minecraft (GLFW/LWJGL) will create a WM_MOUSEMOVE message with (0,0) AND another with (lastX, lastY) 
								  //whenever a mouse button is clicked, WITHOUT calling SetCursorPos
							// - This would cause absoluteCursorPos to be turned on when it shouldn't.
						{
							UpdateAbsoluteCursorCheck();
						}

						if (x != 0)
							lastX = x;

						if (y != 0)
							lastY = y;

						lpMsg->lParam = MAKELPARAM(fakeX, fakeY);
						return 1;
					}
					else
					{
						memset(lpMsg, 0, sizeof(MSG));
						return -1;
					}
				}
				else
				{
					//pMsg->lParam = MAKELPARAM(absoluteX, absoluteY);
					return 1;
				}
			}
			else
			{
				memset(lpMsg, 0, sizeof(MSG));
				return -1;
			}
		}
	}

	//USS signature is 1 << 7 or 0b10000000 for WM_MOUSEMOVE(0x0200). If this is detected, allow event to pass
	if (filterMouseMessages)
	{
		if (Msg == WM_MOUSEMOVE && ((int)_wParam & 0b10000000) > 0)
			return 1;

		// || Msg == 0x00FF
		else if ((Msg >= WM_XBUTTONDOWN && Msg <= WM_XBUTTONDBLCLK) || Msg == WM_MOUSEMOVE || Msg == WM_MOUSEACTIVATE || Msg == WM_MOUSEHOVER || Msg == WM_MOUSELEAVE || Msg == WM_MOUSEWHEEL || Msg == WM_SETCURSOR || Msg == WM_NCMOUSELEAVE)//Other mouse events. 
		{
			memset(lpMsg, 0, sizeof(MSG));
			return -1;
		}
		else
		{
			//if (Msg == WM_ACTIVATE) //0x0006 is WM_ACTIVATE, which resets the mouse position for starbound [citation needed]
			//	return CallNextHookEx(NULL, code, 1, 0);
			//else
			//	return CallNextHookEx(NULL, code, wParam, lParam);
			if (Msg == WM_ACTIVATE)
			{
				lpMsg->lParam = 1;
				lpMsg->wParam = 0;
			}
			return 1;
		}
	}

	return 1;
}

BOOL WINAPI GetMessageA_Hook(LPMSG lpMsg, HWND hWnd, UINT wMsgFilterMin, UINT wMsgFilterMax)
{
	BOOL ret = GetMessageA(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);

	return ret == -1 ? -1 : FilterMessage(lpMsg);
}

BOOL WINAPI GetMessageW_Hook(LPMSG lpMsg, HWND hWnd, UINT wMsgFilterMin, UINT wMsgFilterMax)
{
	BOOL ret = GetMessageW(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);

	return ret == -1 ? -1 : FilterMessage(lpMsg);
}

BOOL WINAPI PeekMessageA_Hook(LPMSG lpMsg, HWND hWnd, UINT wMsgFilterMin, UINT wMsgFilterMax, UINT wRemoveMsg)
{
	BOOL ret = PeekMessageA(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);

	return ret == FALSE ? FALSE : FilterMessage(lpMsg);
}

BOOL WINAPI PeekMessageW_Hook(LPMSG lpMsg, HWND hWnd, UINT wMsgFilterMin, UINT wMsgFilterMax, UINT wRemoveMsg)
{
	BOOL ret = PeekMessageW(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);

	return ret == FALSE ? FALSE : FilterMessage(lpMsg);
}

static BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, LPVOID pvRef)
{
	DIDEVICEINSTANCE di = *lpddi;

	bool adding = false;
	
	/* https://www.usb.org/sites/default/files/documents/hut1_12v2.pdf page 26:
		4 : Joystick
		5 : Game Pad */

	if (di.wUsage == 4 || di.wUsage == 5 && dinputGuids_i < maxDinputDevices)
	{
		dinputGuids[dinputGuids_i++] = di.guidInstance;
		adding = true;
	}

	std::cout << "device enumerate, instanceName=" << di.tszInstanceName << ", productName=" << di.tszProductName << ", usage=" << di.wUsage 
		<< ", usagePage=" << di.wUsagePage << ", adding to list = " << adding << "\n";
	
	return DIENUM_CONTINUE;
}

static BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	LPDIRECTINPUTDEVICE8 did = (LPDIRECTINPUTDEVICE8)pvRef;
	did->Unacquire();

	DIPROPRANGE range;
	range.lMax = 32767;
	range.lMin = -32768;
	range.diph.dwSize = sizeof(DIPROPRANGE);
	range.diph.dwHeaderSize = sizeof(DIPROPHEADER);
	range.diph.dwHow = DIPH_BYID;
	range.diph.dwObj = lpddoi->dwType;

	if (FAILED(did->SetProperty(DIPROP_RANGE, &range.diph)))
		return DIENUM_STOP;
	else
		return DIENUM_CONTINUE;
}

LPDIRECTINPUTDEVICE8 dinputDevice = 0;

//First argument is a pointer to the COM object, required or application crashes after executing hook
HRESULT __stdcall Dinput_GetDeviceState_Hook(LPDIRECTINPUTDEVICE8 pDev, DWORD cbData, LPVOID lpvData)
{
	//std::cout << "Dinput_GetDeviceStates_Hook, cbData=" << cbData << ", lpvData=" << lpvData << "\n";
	return dinputDevice->GetDeviceState(cbData, lpvData);
}

HRESULT __stdcall Dinput_CreateDevice_Hook(IDirectInput8* pdin, REFGUID rguid, LPDIRECTINPUTDEVICE8A* lplpDirectInputDevice, LPUNKNOWN pUnkOuter)
{
	std::cout << "Dinput CreateDeviceHook called\n";
	return pDinput->CreateDevice(controllerGuid, lplpDirectInputDevice, pUnkOuter);
}

void installDinputHook(void* EntryPoint, void* HookProc, string name)
{
	HOOK_TRACE_INFO hHook = { NULL };

	NTSTATUS hookResult = LhInstallHook(
		EntryPoint,
		HookProc,
		NULL,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = { 0 };
		//LhSetExclusiveACL(ACLEntries, 1, &hHook);
		LhSetInclusiveACL(ACLEntries, 1, &hHook);//TODO: switch back to exclusive

		std::cout << "Successfully installed dinput8 hook " << name << "\n";
	}
	else
	{
		std::cout << "Failed to install dinput8 hook " << name << " in module, NTSTATUS: " << hookResult << "\n";
	}
}

void installDinputHooks()
{
	//First 4 byte (32bit) or 8 bytes (64bit) of object with virtual functions will be a pointer to an array of pointers to the virtual functions
#ifdef X64
		using ptrSize = long long;//64 bit pointers
#else
		using ptrSize = int;//32 bit pointers
#endif

	//https://kaisar-haque.blogspot.com/2008/07/c-accessing-virtual-table.html
	ptrSize* vptr_device = *(ptrSize**)dinputDevice;
	ptrSize* vptr_dinput = *(ptrSize**)pDinput;

	using GetDeviceStateFunc = HRESULT(__stdcall *)(DWORD, LPVOID);
	GetDeviceStateFunc GetDeviceStatePointer = (GetDeviceStateFunc)vptr_device[9];
	installDinputHook(GetDeviceStatePointer, Dinput_GetDeviceState_Hook, "GetDeviceState");

	using CreateDeviceFunc = HRESULT(__stdcall *)(REFGUID rguid, LPDIRECTINPUTDEVICE8A* lplpDirectInputDevice, LPUNKNOWN pUnkOuter);
	CreateDeviceFunc CreateDevicePointer = (CreateDeviceFunc)vptr_dinput[3];
	installDinputHook(CreateDevicePointer, Dinput_CreateDevice_Hook, "CreateDevice");
}




extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	//Cout will go to the games console (test easily with SMAPI/Minecraft)
	std::cout << "Injected CPP\n";
	std::cout << "Injected by host process ID: " << inRemoteInfo->HostPID << "\n";
	std::cout << "Passed in data size:" << inRemoteInfo->UserDataSize << "\n";
	
	std::cout << "DllHandle=" << DllHandle << "\n";
	
	InitializeCriticalSection(&mcs);

	int i = 0;
	while (ShowCursor(FALSE) >= -10 || i++ > 20);
	SetCursor(NULL);

	if (inRemoteInfo->UserDataSize == 1024)
	{
		//Get UserData
		BYTE* data = inRemoteInfo->UserData;

		string ipcChannelName = string(reinterpret_cast<char*>(data), 256);
		_ipcChannelNameRead = ipcChannelName;
		std::cout << "Received IPC channel read: " << ipcChannelName << "\n";

		string ipcChannelNameWrite = string(reinterpret_cast<char*>(data+256), 256);
		_ipcChannelNameWrite = ipcChannelNameWrite;
		std::cout << "Received IPC channel write: " << ipcChannelNameWrite << "\n";
		
		hWnd = (HWND)bytesToInt(data + 512);
		std::cout << "Received hWnd: " << hWnd << "\n";

		controllerIndex = bytesToInt(data + 512 + 4);
		std::cout << "Received controller index: " << controllerIndex << "\n";

		allowedMouseHandle = (HANDLE)bytesToInt(data + 512 + 8);
		std::cout << "Allowed mouse handle: " << allowedMouseHandle << "\n";
		
		updateAbsoluteFlagInMouseMessage = *(data + 512 + 12) == 1;
		std::cout << "Update absolute flag in mouse message: " << updateAbsoluteFlagInMouseMessage << "\n";

		BYTE* _p = data +  512 + 12 + 1;

		bool HookGetCursorPos				= *(_p++) == 1;
		bool HookGetForegroundWindow		= *(_p++) == 1;
		bool HookGetAsyncKeyState			= *(_p++) == 1;
		bool HookGetKeyState				= *(_p++) == 1;
		bool HookCallWindowProcW			= *(_p++) == 1;
		bool HookRegisterRawInputDevices	= *(_p++) == 1;
		bool HookSetCursorPos				= *(_p++) == 1;
		bool HookXInput						= *(_p++) == 1;
		bool useLegacyInput					= *(_p++) == 1;
		bool hookMouseVisibility			= *(_p++) == 1;

		std::cout << "HookGetCursorPos: " << HookGetCursorPos << "\n";
		std::cout << "HookGetForegroundWindow: " << HookGetForegroundWindow << "\n";
		std::cout << "HookGetAsyncKeyState: " << HookGetAsyncKeyState << "\n";
		std::cout << "HookGetKeyState: " << HookGetKeyState << "\n";
		std::cout << "HookCallWindowProcW: " << HookCallWindowProcW << "\n";
		std::cout << "HookRegisterRawInputDevices: " << HookRegisterRawInputDevices << "\n";
		std::cout << "HookSetCursorPos: " << HookSetCursorPos << "\n";
		std::cout << "HookXInput: " << HookXInput << "\n";
		std::cout << "useLegacyInput: " << useLegacyInput << "\n";
		std::cout << "hookMouseVisibility: " << hookMouseVisibility << "\n";

		//Install hooks
		if (HookGetForegroundWindow) 
		{
			installHook(TEXT("user32"), "GetForegroundWindow", GetForegroundWindow_Hook);
			installHook(TEXT("user32"), "WindowFromPoint", WindowFromPoint_Hook);
			installHook(TEXT("user32"), "GetActiveWindow", GetActiveWindow_Hook);
			installHook(TEXT("user32"), "IsWindowEnabled", IsWindowEnabled_Hook);
		}

		if (hookMouseVisibility)
		{
			installHook(TEXT("user32"), "ShowCursor", ShowCursor_Hook);
			installHook(TEXT("user32"), "SetCursor", SetCursor_Hook);
		}

		if (HookGetCursorPos)				installHook(TEXT("user32"), "GetCursorPos",				GetCursorPos_Hook);
		if (HookGetAsyncKeyState)			installHook(TEXT("user32"), "GetAsyncKeyState",			GetAsyncKeyState_Hook);
		if (HookGetKeyState)				installHook(TEXT("user32"), "GetKeyState",				GetKeyState_Hook);
		if (HookSetCursorPos)				installHook(TEXT("user32"), "SetCursorPos",				SetCursorPos_Hook);
		//if (filterRawInput)						installHook(TEXT("user32"), "RegisterRawInputDevices",	RegisterRawInputDevices_Hook);

		//Hook XInput dll
		if (HookXInput)
		{
			LPCSTR xinputNames[] = { "xinput1_3.dll", "xinput1_4.dll", "xinput1_2.dll", "xinput1_1.dll", "xinput9_1_0.dll" };

			for (int xi = 0; xi < 5; xi++)
			{
				if (GetModuleHandleA(xinputNames[xi]) != NULL)
				{
					installHook(xinputNames[xi], "XInputGetState", XInputGetState_Hook);
					installHook(xinputNames[xi], "XInputSetState", XInputSetState_Hook);
				}
				else
				{
					std::cout << "Not hooking " << xinputNames[xi] << " because not loaded\n";
				}
			}
		}

		filterRawInput = HookRegisterRawInputDevices;
		filterMouseMessages = HookCallWindowProcW;
		//if (filterRawInput || filterMouseMessages)	installHook(TEXT("user32"), "CallWindowProcW",			CallWindowProc_Hook);
		
		if (filterRawInput || filterMouseMessages || enableLegacyInput)
		{
			installHook(TEXT("user32"), "GetMessageA", GetMessageA_Hook);
			installHook(TEXT("user32"), "GetMessageW", GetMessageW_Hook);

			installHook(TEXT("user32"), "PeekMessageA", PeekMessageA_Hook);
			installHook(TEXT("user32"), "PeekMessageW", PeekMessageW_Hook);
		}
		
		if (controllerIndex == 0)
		{
			std::cout << "Not setting up dinput: controllerIndex == 0\n";
		}
		else
		{
			dinputDevice = 0;

			HRESULT dinput_ret = DirectInput8Create(DllHandle, DIRECTINPUT_VERSION, IID_IDirectInput8, (void**)&(pDinput), NULL);

			if (dinput_ret != DI_OK)
			{
				std::cerr << "Fail DirectInput8Create, dinput_ret=" << dinput_ret << endl;
			}
			else
			{
				std::cout << "Succeed dDirectInput8Create\n";
				dinputGuids_i = 0;
				pDinput->EnumDevices(DI8DEVCLASS_ALL, DIEnumDevicesCallback, 0, DIEDFL_ALLDEVICES);
				//TODO: sort the array by guidInstance (important so order is same in all hooks in games)

				if (!(controllerIndex <= maxDinputDevices && controllerIndex <= dinputGuids_i))
				{
					std::cerr << "Not selecting dinput controller because controllerIndex out of range" << endl;
					MessageBox(NULL, "Not selecting dinput controller because controllerIndex out of range", NULL, MB_OK);
				}
				else
				{
					controllerGuid = dinputGuids[controllerIndex - 1];
					HRESULT cdRes = pDinput->CreateDevice(controllerGuid, &dinputDevice, NULL);
					
					if (cdRes != DI_OK)
					{
						std::cerr << "dinput create device error: " << cdRes << endl;
					}
					else
					{
						//Dinput8 hook
						installDinputHooks();

						dinputDevice->SetCooperativeLevel(hWnd, DISCL_BACKGROUND | DISCL_NONEXCLUSIVE);

						dinputDevice->SetDataFormat(&c_dfDIJoystick);

						DIDEVCAPS caps;
						caps.dwSize = sizeof(DIDEVCAPS);
						HRESULT gcRes = dinputDevice->GetCapabilities(&caps);

						std::cout << "dinput device number of buttons =" << caps.dwButtons << "\n";
						std::cout << "dinput device number of axes =" << caps.dwAxes << "\n";


						dinputDevice->EnumObjects(&DIEnumDeviceObjectsCallback, dinputDevice, DIDFT_AXIS);


						HRESULT aquireResult = dinputDevice->Acquire();

						if (aquireResult == DI_OK)
							std::cout << "Successfully aquired dinput device\n";
						else
							std::cout << "Failed to aquired dinput device\n";
					}
				}
			}
		}

		//Start named pipe client
		startPipeListen();

		if (dinputDevice != 0)
		{
			dinputDevice->Unacquire();
			dinputDevice->Release();
		}
	}
	else
	{
		std::cout << "Failed getting user data\nExpected size 1024, Received "<< (inRemoteInfo->UserDataSize)<<"\n";
	}

	return;
}