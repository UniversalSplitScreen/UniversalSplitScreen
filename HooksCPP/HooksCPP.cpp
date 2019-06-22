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
using namespace std;

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


// If enabled, will alternate between absolute mouse position (bound to 0,0 and width,height) and delta mouse position (returns the changes, without bounding)
bool enableLegacyInput = true; 

BOOL useAbsoluteCursorPos = TRUE;
time_t timeSinceLastSetCursorPos;

//Time since the last SetCursorPos that we will assume the game is in a UI menu and needs absolute mouse position
//(Technically one second since it records as an time_t).
const double minTimeForAbs = 0.5;


UINT16 vkey_state;//Stores the mouse keys (5 of them) and the WASD keys. (1=on, 0=off)
int controllerIndex = 0;//The controller index for this game.
HANDLE allowedMouseHandle = 0;//We will allow raw input from this mouse handle.

bool filterRawInput;
bool filterMouseMessages;

bool pipeClosed = false;

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

		if (enableLegacyInput && useAbsoluteCursorPos == FALSE)
		{
			double dt = difftime(time(NULL), timeSinceLastSetCursorPos);
			if (dt >= minTimeForAbs)
			{
				//It's been minTimeForAbs since last SetCursorPos, so we assume we're in a menu and need absolute cursor pos
				useAbsoluteCursorPos = TRUE;
			}
		}
		
	}
	return true;
}

BOOL WINAPI SetCursorPos_Hook(int X, int Y)
{
	POINT p;
	p.x = X;
	p.y = Y;

	//SetCursorPos require screen coordinates (relative to 0,0 of monitor)
	ScreenToClient(hWnd, &p);

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

		time(&timeSinceLastSetCursorPos);
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
					std::cout << "Received pipe closed message. Closing pipe..." << endl;
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

//Passed as a void* from InjectorCPP
struct UserData
{
	HWND hWnd;
	char ipcChannelNameRead[256];//Name will be 30 characters
	char ipcChannelNameWrite[256];//Name will be 30 characters
	int controllerIndex;
	int allowedMouseHandle;
	bool HookGetCursorPos;
	bool HookGetForegroundWindow;
	bool HookGetAsyncKeyState;
	bool HookGetKeyState;
	bool HookCallWindowProcW;
	bool HookRegisterRawInputDevices;
	bool HookSetCursorPos;
	bool HookXInput;
	bool useLegacyInput;
	bool hookMouseVisibility;
};

LRESULT CALLBACK CallMsgProc(_In_ int code, _In_ WPARAM wParam, _In_ LPARAM lParam)
{
	MSG* pMsg = (MSG*)lParam;
	UINT Msg = pMsg->message;
	LPARAM _lParam = pMsg->lParam;
	WPARAM _wParam = pMsg->wParam;
	
	const LRESULT blockRet = 0;

	if ((filterRawInput) && (Msg == WM_INPUT) && (allowedMouseHandle != 0))
	{
		UINT dwSize = sizeof(RAWINPUTHEADER);
		{
			RAWINPUT raw[sizeof(RAWINPUTHEADER)];

			if (GetRawInputData((HRAWINPUT)_lParam, RID_HEADER, raw, &dwSize, sizeof(RAWINPUTHEADER)) == dwSize)
			{
				if (raw->header.dwType == RIM_TYPEMOUSE)
				{
					if (raw->header.hDevice == allowedMouseHandle)
					{
						return CallNextHookEx(NULL, code, wParam, lParam);//Wastes CPU (?)
					}
					else
					{
						pMsg->message = WM_NULL;
						return blockRet;
					}
				}
			}
		}
	}

	//USS signature is 1 << 7 or 0b10000000 for WM_MOUSEMOVE(0x0200). If this is detected, allow event to pass
	if (filterMouseMessages)
	{
		if (Msg == WM_MOUSEMOVE && ((int)_wParam & 0b10000000) > 0)
			return CallNextHookEx(NULL, code, wParam, lParam);

		// || Msg == 0x00FF
		else if ((Msg >= WM_XBUTTONDOWN && Msg <= WM_XBUTTONDBLCLK) || Msg == WM_MOUSEMOVE || Msg == WM_MOUSEACTIVATE || Msg == WM_MOUSEHOVER || Msg == WM_MOUSELEAVE || Msg == WM_MOUSEWHEEL || Msg == WM_SETCURSOR)//Other mouse events. 
		{
			//pCwp->message = WM_NULL;
			pMsg->message = WM_NULL;
			return blockRet;
		}
		else
		{
			if (Msg == WM_ACTIVATE) //0x0006 is WM_ACTIVATE, which resets the mouse position for starbound [citation needed]
				return CallNextHookEx(NULL, code, 1, 0);
			else
				return CallNextHookEx(NULL, code, wParam, lParam);
		}
	}

	if (enableLegacyInput && Msg == WM_MOUSEMOVE)
	{
		if (useAbsoluteCursorPos && ((int)_wParam & 0b10000000) > 0)
			return CallNextHookEx(NULL, code, wParam, lParam);//This is from USS. We should pass this as it is the absolute pos.
		else
		{
			pMsg->message = WM_NULL;
			return blockRet;
		}
	}

	return CallNextHookEx(NULL, code, wParam, lParam);//Pass
}

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
			cout << "scv fail, err=" << GetLastError() << endl;
		}

		sentVisibility = show;
	}

	//std::cout << "scvr, b="<<show << endl;
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

extern "C" __declspec(dllexport) void __stdcall NativeInjectionEntryPoint(REMOTE_ENTRY_INFO* inRemoteInfo)
{
	//Cout will go to the games console (test easily with SMAPI/Minecraft)
	std::cout << "Injected CPP\n";
	std::cout << "Injected by host process ID: " << inRemoteInfo->HostPID << "\n";
	std::cout << "Passed in data size:" << inRemoteInfo->UserDataSize << "\n";
	
	std::cout << "DllHandle=" << DllHandle << endl;
	
	InitializeCriticalSection(&mcs);

	int i = 0;
	while (ShowCursor(FALSE) >= -10 || i++ > 20);
	SetCursor(NULL);

	if (inRemoteInfo->UserDataSize == sizeof(UserData))
	{
		//Get UserData
		UserData userData = *reinterpret_cast<UserData *>(inRemoteInfo->UserData);

		hWnd = userData.hWnd;
		std::cout << "Received hWnd: " << hWnd << endl;

		string ipcChannelName(userData.ipcChannelNameRead);
		_ipcChannelNameRead = ipcChannelName;
		std::cout << "Received IPC channel read: " << ipcChannelName << endl;

		string ipcChannelNameWrite(userData.ipcChannelNameWrite);
		_ipcChannelNameWrite = ipcChannelNameWrite;
		std::cout << "Received IPC channel write: " << ipcChannelNameWrite << endl;

		controllerIndex = userData.controllerIndex;
		std::cout << "Received controller index: " << controllerIndex << endl;

		allowedMouseHandle = (HANDLE)userData.allowedMouseHandle;
		std::cout << "Allowed mouse handle: " << allowedMouseHandle << endl;
		
		enableLegacyInput = userData.useLegacyInput;
		std::cout << "Use legacy input: " << enableLegacyInput << endl;

		//Install hooks

		if (userData.HookGetForegroundWindow) 
		{
			installHook(TEXT("user32"), "GetForegroundWindow", GetForegroundWindow_Hook);
			installHook(TEXT("user32"), "WindowFromPoint", WindowFromPoint_Hook);
			installHook(TEXT("user32"), "GetActiveWindow", GetActiveWindow_Hook);
			installHook(TEXT("user32"), "IsWindowEnabled", IsWindowEnabled_Hook);
		}

		if (userData.hookMouseVisibility)
		{
			installHook(TEXT("user32"), "ShowCursor", ShowCursor_Hook);
			installHook(TEXT("user32"), "SetCursor", SetCursor_Hook);
		}

		if (userData.HookGetCursorPos)				installHook(TEXT("user32"), "GetCursorPos",				GetCursorPos_Hook);
		if (userData.HookGetAsyncKeyState)			installHook(TEXT("user32"), "GetAsyncKeyState",			GetAsyncKeyState_Hook);
		if (userData.HookGetKeyState)				installHook(TEXT("user32"), "GetKeyState",				GetKeyState_Hook);
		if (userData.HookSetCursorPos)				installHook(TEXT("user32"), "SetCursorPos",				SetCursorPos_Hook);
		//if (filterRawInput)						installHook(TEXT("user32"), "RegisterRawInputDevices",	RegisterRawInputDevices_Hook);

		//Hook XInput dll
		if (userData.HookXInput)
		{
			LPCSTR xinputNames[] = { "xinput1_3.dll", "xinput1_4.dll", "xinput1_2.dll", "xinput1_1.dll", "xinput9_1_0.dll" };

			for (int xi = 0; xi < 5; xi++)
			{
				installHook(xinputNames[xi], "XInputGetState", XInputGetState_Hook);
				installHook(xinputNames[xi], "XInputSetState", XInputSetState_Hook);
			}
		}

		filterRawInput = userData.HookRegisterRawInputDevices;
		filterMouseMessages = userData.HookCallWindowProcW;
		//if (filterRawInput || filterMouseMessages)	installHook(TEXT("user32"), "CallWindowProcW",			CallWindowProc_Hook);
		
		if (filterRawInput || filterMouseMessages || enableLegacyInput)
		{
			HHOOK hhook = SetWindowsHookEx(WH_GETMESSAGE, CallMsgProc, DllHandle, 0);
			std::cout << "hhook = " << hhook << ", GetLastError=" << GetLastError() << endl;
		}

		//Start named pipe client
		startPipeListen();
	}
	else
	{
		std::cout << "Failed getting user data\n";
	}

	return;
}