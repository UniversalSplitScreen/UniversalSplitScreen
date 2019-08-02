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
#include <winuser.h>
using namespace std;

#if _WIN64
#define X64
#else
#define X86
#endif

extern HMODULE DllHandle;

HWND hWnd = nullptr;
string _ipcChannelNameRead; //The name of the named pipe.
string _ipcChannelNameWrite;

HANDLE hPipeRead;
HANDLE hPipeWrite;

CRITICAL_SECTION mcs;
int fake_x; //Delta X
int fake_y;

int absolute_x;
int absolute_y;

bool update_absolute_flag_in_mouse_message;

// If enabled, will alternate between absolute mouse position (bound to 0,0 and width,height) and delta mouse position (returns the changes, without bounding)
bool enable_legacy_input = true;

BOOL use_absolute_cursor_pos = TRUE;
int use_absolute_cursor_pos_counter = 0; // 0/1/2/3/... : FALSE, requiredAbsCount : TRUE
const int required_abs_count = 40; //Requires higher number for higher mouse polling rate

//time_t timeSinceLastSetCursorPos;
//Time since the last SetCursorPos that we will assume the game is in a UI menu and needs absolute mouse position
//const double minTimeForAbs = 0;


UINT16 vkey_state; //Stores the mouse keys (5 of them) and the WASD keys. (1=on, 0=off)

BYTE* vkeys_state = new BYTE[256 / 8]; //256 vkeys, 8 bits per byte


int controller_index = 0; //The controller index for this game.
HANDLE allowed_mouse_handle = nullptr; //We will allow raw input from this mouse handle.

bool filter_raw_input;
bool filter_mouse_messages;

bool pipe_closed = false;

static IDirectInput8* p_dinput;

bool HookDinput = false;
const int max_dinput_devices = 16;
static GUID dinput_guids[max_dinput_devices];
static GUID controller_guid;
int dinputGuids_i = 0;
bool dinput_block_input = false;
static LPDIRECTINPUTDEVICE8 dinput_device = nullptr;
LONG dinput_range_max = 32767;
LONG dinput_range_min = -32768;
int dinput_device_data_format = 2; //c_dfDIJoystick : 1, c_dfDIJoystick2 : 2
int dinput_device_data_format7 = 2; //for dinput7
LPDIRECTINPUT7 p_dinput7;
LPDIRECTINPUTDEVICE p_ctrlr7;

void update_absolute_cursor_check()
{
	if (enable_legacy_input && use_absolute_cursor_pos == FALSE)
	{
		//double dt = difftime(time(NULL), timeSinceLastSetCursorPos);
		//if (dt >= minTimeForAbs)
		//{

		//We assume we're in a menu and need absolute cursor pos
		use_absolute_cursor_pos_counter++;
		if (use_absolute_cursor_pos_counter == required_abs_count)
		{
			use_absolute_cursor_pos = TRUE;
		}

		//}
	}
}

BOOL WINAPI GetCursorPos_Hook(LPPOINT lpPoint)
{
	if (lpPoint)
	{
		EnterCriticalSection(&mcs);
		if ((!enable_legacy_input) || use_absolute_cursor_pos == TRUE)
		{
			//Absolute mouse position (always do this if legacy input is off)
			lpPoint->x = absolute_x;
			lpPoint->y = absolute_y;
		}
		else
		{
			//Delta mouse position
			lpPoint->x = fake_x;
			lpPoint->y = fake_y;
		}

		LeaveCriticalSection(&mcs);
		ClientToScreen(hWnd, lpPoint);

		update_absolute_cursor_check();
	}
	return true;
}

int origin_x, origin_y;

BOOL WINAPI SetCursorPos_Hook(int X, int Y)
{
	POINT p;
	p.x = X;
	p.y = Y;

	//SetCursorPos require screen coordinates (relative to 0,0 of monitor)
	ScreenToClient(hWnd, &p);

	origin_x = p.x;
	origin_y = p.y;

	if (!enable_legacy_input)
	{
		EnterCriticalSection(&mcs);
		absolute_x = p.x;
		absolute_y = p.y;
		LeaveCriticalSection(&mcs);
	}
	else
	{
		EnterCriticalSection(&mcs);
		fake_x = p.x;
		fake_y = p.y;
		LeaveCriticalSection(&mcs);

		//time(&timeSinceLastSetCursorPos);
		use_absolute_cursor_pos_counter = 0;
		use_absolute_cursor_pos = FALSE;
	}

	return TRUE;
}

HWND WINAPI GetForegroundWindow_Hook()
{
	return hWnd;
}

HWND WINAPI WindowFromPoint_Hook(POINT Point)
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

HWND WINAPI GetFocus_Hook()
{
	return hWnd;
}

inline bool is_vkey_down(int vkey)
{
	if (vkey >= 0xFF) return false;

	BYTE p = vkeys_state[vkey / 8];
	bool ret = (p & (1 << (vkey % 8))) != 0;

	if (!ret)
	{
		if (vkey == VK_LSHIFT || vkey == VK_RSHIFT) return is_vkey_down(VK_SHIFT);
		if (vkey == VK_LMENU || vkey == VK_RMENU) return is_vkey_down(VK_MENU); //alt
		if (vkey == VK_LCONTROL || vkey == VK_RCONTROL) return is_vkey_down(VK_CONTROL);
	}

	return ret;
}

inline void setVkeyState(int vkey, bool down)
{
	if (vkey >= 0xFF) return;

	BYTE* p = vkeys_state + (vkey / 8);
	int shift = (1 << (vkey % 8));
	if (down)
		*p |= shift;
	else
		*p &= (~shift);

	if (vkey == VK_LSHIFT || vkey == VK_RSHIFT) setVkeyState(VK_SHIFT, down);
	else if (vkey == VK_LMENU || vkey == VK_RMENU) setVkeyState(VK_MENU, down);
	else if (vkey == VK_LCONTROL || vkey == VK_RCONTROL) setVkeyState(VK_CONTROL, down);
}

SHORT WINAPI GetAsyncKeyState_Hook(int vKey)
{
	return is_vkey_down(vKey) ? 0b1000000000000000 : 0;
}

SHORT WINAPI GetKeyState_Hook(int nVirtKey)
{
	return is_vkey_down(nVirtKey) ? 0b1000000000000000 : 0;
}

BOOL WINAPI RegisterRawInputDevices_Hook(PCRAWINPUTDEVICE pRawInputDevices, UINT uiNumDevices, UINT cbSize)
{
	//Don't actually let raw input be registered, but pretend to the game it has
	return true;
}

DWORD packetNumber = 0;
DWORD WINAPI XInputGetState_Hook(DWORD dwUserIndex, XINPUT_STATE* pState)
{
	if (controller_index == 0) // user wants no controller on this game
		return ERROR_DEVICE_NOT_CONNECTED;
		
	if (!HookDinput)
	{
		return XInputGetState(controller_index - 1, pState);
	}

	pState->dwPacketNumber = packetNumber++;
	memset(&(pState->Gamepad), 0, sizeof(XINPUT_GAMEPAD));

	dinput_device->Poll();
	DIJOYSTATE2 diState;
	dinput_device->GetDeviceState(sizeof(DIJOYSTATE2), &diState);

#define BTN(n, f) if (diState.rgbButtons[n] != 0) pState->Gamepad.wButtons |= f
	BTN(0, XINPUT_GAMEPAD_A);
	BTN(1, XINPUT_GAMEPAD_B);
	BTN(2, XINPUT_GAMEPAD_X);
	BTN(3, XINPUT_GAMEPAD_Y);
	BTN(4, XINPUT_GAMEPAD_LEFT_SHOULDER);
	BTN(5, XINPUT_GAMEPAD_RIGHT_SHOULDER);
	BTN(6, XINPUT_GAMEPAD_BACK);
	BTN(7, XINPUT_GAMEPAD_START);
	BTN(8, XINPUT_GAMEPAD_LEFT_THUMB);
	BTN(9, XINPUT_GAMEPAD_RIGHT_THUMB);
#undef BTN

	const auto pov = diState.rgdwPOV;
	if (!(LOWORD(pov[0]) == 0xFFFF))//POV not centred
	{
		auto deg = (pov[0]) / 4500;
#define DPAD(a,b,c, f) if (deg == (a) || deg == (b) || deg == (c)) pState->Gamepad.wButtons |= f
		DPAD(7, 0, 1, XINPUT_GAMEPAD_DPAD_UP);
		DPAD(1, 2, 3, XINPUT_GAMEPAD_DPAD_RIGHT);
		DPAD(3, 4, 5, XINPUT_GAMEPAD_DPAD_DOWN);
		DPAD(5, 6, 7, XINPUT_GAMEPAD_DPAD_LEFT);
#undef DPAD
	}

#define DEADZONE(x, d) (((x) >= (d) || (x) <= (-(d))) ? (x) : 0)
	pState->Gamepad.sThumbLX = DEADZONE(diState.lX, XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE);
	pState->Gamepad.sThumbLY = -1 - DEADZONE(diState.lY, XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE);
	pState->Gamepad.sThumbRX = DEADZONE(diState.lRx, XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE);
	pState->Gamepad.sThumbRY = -1 - DEADZONE(diState.lRy, XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE);
#undef DEADZONE

#define TRIGGERDEADZONE(x) (((x) >= XINPUT_GAMEPAD_TRIGGER_THRESHOLD) ? (x) : 0)
	const auto triggerAxis = diState.lZ;
	if (triggerAxis > 0)
	{
		const auto x = 255 * ((float)triggerAxis / dinput_range_max);
		pState->Gamepad.bLeftTrigger = TRIGGERDEADZONE(x);
	}
	else if (triggerAxis < 0)
	{
		const auto x = 255 * ((float)triggerAxis / dinput_range_min);
		pState->Gamepad.bRightTrigger = TRIGGERDEADZONE(x);
	}
#undef TRIGGERDEADZONE

	return ERROR_SUCCESS;
}

DWORD WINAPI XInputSetState_Hook(DWORD dwUserIndex, XINPUT_VIBRATION* pVibration)
{
	if (controller_index == 0)
		return ERROR_DEVICE_NOT_CONNECTED;
	return XInputSetState(controller_index - 1, pVibration);
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
		nullptr,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		nullptr
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
		nullptr,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		nullptr
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
		BYTE buffer[9]; //9 bytes are sent at a time (1st is message, next 8 for 2 ints)
		DWORD bytesRead = 0;

		BOOL result = ReadFile(
			hPipeRead,
			buffer,
			9 * sizeof(BYTE),
			&bytesRead,
			nullptr
		);

		if (result && bytesRead == 9)
		{
			int param1 = bytesToInt(&buffer[1]);

			int param2 = bytesToInt(&buffer[5]);

			//cout << "Received message. Msg=" << (int)buffer[0] << ", param1=" << param1 << ", param2=" << param2 << "\n";

			switch (buffer[0])
			{
			case 0x01: //Add delta cursor pos
				{
					EnterCriticalSection(&mcs);
					fake_x += param1;
					fake_y += param2;
					LeaveCriticalSection(&mcs);
					break;
				}
			case 0x04: //Set absolute cursor pos
				{
					EnterCriticalSection(&mcs);
					absolute_x = param1;
					absolute_y = param2;
					LeaveCriticalSection(&mcs);
					break;
				}
			case 0x02: //Set VKey
				{
					setVkeyState(param1, param2 != 0);
					break;
				}
			case 0x03: //Close named pipe
				{
					std::cout << "Received pipe closed message. Closing pipe..." << "\n";
					pipe_closed = true;
					return;
				}
			case 0x05: //Focus desktop
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
	HOOK_TRACE_INFO hHook = {nullptr};

	NTSTATUS hookResult = LhInstallHook(
		GetProcAddress(GetModuleHandle(moduleHandle), lpProcName),
		InCallback,
		nullptr,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = {0};
		LhSetExclusiveACL(ACLEntries, 1, &hHook);
		std::cout << "Successfully installed hook " << lpProcName << " in module '" << moduleHandle << "'\n";
	}
	else
	{
		std::cout << "Failed to install hook " << lpProcName << " in module '" << moduleHandle << "', NTSTATUS: " <<
			hookResult << "\n";
	}

	return hookResult;
}

int lastX, lastY;

bool sentVisibility = true; //the visibility stored in C#

void SetCursorVisibility(bool show)
{
	if (show != sentVisibility)
	{
		BYTE buffer[9] = {0x06, 0, 0, 0, (show ? 1 : 0), 0, 0, 0, 0};

		DWORD bytesRead = 0;

		BOOL result = WriteFile(
			hPipeWrite,
			buffer,
			9 * sizeof(BYTE),
			&bytesRead,
			nullptr
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
	SetCursorVisibility(hCursor != nullptr);
	if (hCursor == nullptr) SetCursor(nullptr);
	return hCursor;
	//return SetCursor(hCursor);
}

BOOL FilterMessage(LPMSG lpMsg)
{
	UINT Msg = lpMsg->message;
	WPARAM _wParam = lpMsg->wParam;
	LPARAM _lParam = lpMsg->lParam;

	//Filter raw input
	if (Msg == WM_INPUT && filter_raw_input)
	{
		UINT dwSize = 0;
		const UINT sorh = sizeof(RAWINPUTHEADER);
		static RAWINPUT raw[sorh];

		if ((0 == GetRawInputData((HRAWINPUT)lpMsg->lParam, RID_HEADER, nullptr, &dwSize, sorh)) &&
			(dwSize == sorh) &&
			(dwSize == GetRawInputData((HRAWINPUT)lpMsg->lParam, RID_HEADER, raw, &dwSize, sorh)) &&
			(raw->header.dwType == RIM_TYPEMOUSE))
		{
			if (raw->header.hDevice == allowed_mouse_handle)
			{
				return 1;
			}
			memset(lpMsg, 0, sizeof(MSG));
			return -1;
		}
	}

	//Legacy input filter
	if (enable_legacy_input)
	{
		if (Msg == WM_MOUSEMOVE)
		{
			if (((int)_wParam & 0b10000000) > 0) //Signature for message sent from USS (C#)
			{
				if (use_absolute_cursor_pos == FALSE)
				{
					if (update_absolute_flag_in_mouse_message)
					{
						int x = GET_X_LPARAM(_lParam);
						int y = GET_Y_LPARAM(_lParam);

						if (!(x == 0 && y == 0) && !(x == lastX && y == lastY))
							// - Minecraft (GLFW/LWJGL) will create a WM_MOUSEMOVE message with (0,0) AND another with (lastX, lastY) 
							//whenever a mouse button is clicked, WITHOUT calling SetCursorPos
							// - This would cause absoluteCursorPos to be turned on when it shouldn't.
						{
							update_absolute_cursor_check();
						}

						if (x != 0)
							lastX = x;

						if (y != 0)
							lastY = y;

						lpMsg->lParam = MAKELPARAM(fake_x, fake_y);
						return 1;
					}
					memset(lpMsg, 0, sizeof(MSG));
					return -1;
				}
				//pMsg->lParam = MAKELPARAM(absoluteX, absoluteY);
				return 1;
			}
			memset(lpMsg, 0, sizeof(MSG));
			return -1;
		}
	}

	//USS signature is 1 << 7 or 0b10000000 for WM_MOUSEMOVE(0x0200). If this is detected, allow event to pass
	if (filter_mouse_messages)
	{
		if (Msg == WM_MOUSEMOVE && ((int)_wParam & 0b10000000) > 0)
			return 1;

		// || Msg == 0x00FF
		if ((Msg >= WM_XBUTTONDOWN && Msg <= WM_XBUTTONDBLCLK) || Msg == WM_MOUSEMOVE || Msg == WM_MOUSEACTIVATE || Msg
			== WM_MOUSEHOVER || Msg == WM_MOUSELEAVE || Msg == WM_MOUSEWHEEL || Msg == WM_SETCURSOR || Msg ==
			WM_NCMOUSELEAVE) //Other mouse events. 
		{
			memset(lpMsg, 0, sizeof(MSG));
			return -1;
		}
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

	if ((di.wUsage == 4 || di.wUsage == 5) && dinputGuids_i < max_dinput_devices)
	{
		std::cout << "inserting (next) at dinputGuids_i=" << dinputGuids_i << "\n";
		dinput_guids[dinputGuids_i++] = di.guidInstance;
		adding = true;
	}

	std::cout << "device enumerate, instanceName=" << di.tszInstanceName << ", productName=" << di.tszProductName <<
		", usage=" << di.wUsage
		<< ", usagePage=" << di.wUsagePage << ", adding to list = " << adding << "\n";

	return DIENUM_CONTINUE;
}

static BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	LPDIRECTINPUTDEVICE8 did = (LPDIRECTINPUTDEVICE8)pvRef;
	did->Unacquire();

	DIPROPRANGE range;
	range.lMax = dinput_range_max; //32767
	range.lMin = dinput_range_min; //-32768
	range.diph.dwSize = sizeof(DIPROPRANGE);
	range.diph.dwHeaderSize = sizeof(DIPROPHEADER);
	range.diph.dwHow = DIPH_BYID;
	range.diph.dwObj = lpddoi->dwType;

	if (FAILED(did->SetProperty(DIPROP_RANGE, &range.diph)))
		return DIENUM_STOP;
	return DIENUM_CONTINUE;
}

static BOOL CALLBACK DIEnumDeviceObjectsCallback7(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	LPDIRECTINPUTDEVICE did = (LPDIRECTINPUTDEVICE)pvRef;
	did->Unacquire();

	DIPROPRANGE range;
	range.lMax = dinput_range_max; //32767
	range.lMin = dinput_range_min; //-32768
	range.diph.dwSize = sizeof(DIPROPRANGE);
	range.diph.dwHeaderSize = sizeof(DIPROPHEADER);
	range.diph.dwHow = DIPH_BYID;
	range.diph.dwObj = lpddoi->dwType;

	if (FAILED(did->SetProperty(DIPROP_RANGE, &range.diph)))
		return DIENUM_STOP;
	return DIENUM_CONTINUE;
}

static BOOL CALLBACK DIEnumDeviceObjectsCallback_CopyProperties(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	LPDIRECTINPUTDEVICE8 theirDevice = (LPDIRECTINPUTDEVICE8)pvRef;

	DIPROPRANGE range;
	range.diph.dwSize = sizeof(DIPROPRANGE);
	range.diph.dwHeaderSize = sizeof(DIPROPHEADER);
	range.diph.dwHow = DIPH_BYID;
	range.diph.dwObj = lpddoi->dwType;

	if (FAILED(theirDevice->GetProperty(DIPROP_RANGE, &range.diph)))
		return DIENUM_STOP;

	dinput_range_max = range.lMax;
	dinput_range_min = range.lMin;

	//seems to return min = 0 and max doubled. Correct for that here:
	if (range.lMin == 0)
	{
		dinput_range_max = range.lMax / 2;
		dinput_range_min = -(range.lMax / 2) - 1;
	}

	return DIENUM_CONTINUE;
}

static BOOL CALLBACK DIEnumDeviceObjectsCallback_CopyProperties7(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	IDirectInputDeviceA* theirDevice = (IDirectInputDeviceA*)pvRef;

	DIPROPRANGE range;
	range.diph.dwSize = sizeof(DIPROPRANGE);
	range.diph.dwHeaderSize = sizeof(DIPROPHEADER);
	range.diph.dwHow = DIPH_BYID;
	range.diph.dwObj = lpddoi->dwType;

	if (FAILED(theirDevice->GetProperty(DIPROP_RANGE, &range.diph)))
		return DIENUM_STOP;

	dinput_range_max = range.lMax;
	dinput_range_min = range.lMin;

	//seems to return min = 0 and max doubled. Correct for that here:
	if (range.lMin == 0)
	{
		dinput_range_max = range.lMax / 2;
		dinput_range_min = -(range.lMax / 2) - 1;
	}

	return DIENUM_CONTINUE;
}

bool hasSetupDinputDeviceProperties = false;

void setupDinputDeviceProperties(LPDIRECTINPUTDEVICE8 theirDevice)
{
	//theirDevice->Unacquire();
	theirDevice->EnumObjects(&DIEnumDeviceObjectsCallback_CopyProperties, theirDevice, DIDFT_AXIS);
	//theirDevice->Acquire();

	dinput_device->Unacquire();
	dinput_device->EnumObjects(&DIEnumDeviceObjectsCallback, dinput_device, DIDFT_AXIS);
	dinput_device->Acquire();

	hasSetupDinputDeviceProperties = true;
}

bool hasSetupDinputDeviceProperties7 = false;

void setupDinputDeviceProperties7(IDirectInputDeviceA* theirDevice)
{
	//theirDevice->Unacquire();
	theirDevice->EnumObjects(&DIEnumDeviceObjectsCallback_CopyProperties7, theirDevice, DIDFT_AXIS);
	//theirDevice->Acquire();

	p_ctrlr7->Unacquire();
	p_ctrlr7->EnumObjects(&DIEnumDeviceObjectsCallback7, p_ctrlr7, DIDFT_AXIS);
	p_ctrlr7->Acquire();

	hasSetupDinputDeviceProperties7 = true;
}

//First argument is a pointer to the COM object, required or application crashes after executing hook
HRESULT __stdcall Dinput_GetDeviceState_Hook(LPDIRECTINPUTDEVICE8 pDev, DWORD cbData, LPVOID lpvData)
{
	if (dinput_block_input)
	{
		memset(lpvData, 0, cbData);
		return DI_OK;
	}
	if (dinput_device_data_format == 1 && cbData == sizeof(DIJOYSTATE2))
	{
		dinput_device->Unacquire();
		dinput_device->SetDataFormat(&c_dfDIJoystick2);
		dinput_device->Acquire();
		dinput_device_data_format = 2;
	}
	else if (dinput_device_data_format == 2 && cbData == sizeof(DIJOYSTATE))
	{
		dinput_device->Unacquire();
		dinput_device->SetDataFormat(&c_dfDIJoystick);
		dinput_device->Acquire();
		dinput_device_data_format = 1;
	}


	if (!hasSetupDinputDeviceProperties) setupDinputDeviceProperties(pDev);
	dinput_device->Poll();
	return dinput_device->GetDeviceState(cbData, lpvData);
}

HRESULT __stdcall Dinput_GetDeviceState_Hook7(IDirectInputDeviceA* pDev, DWORD cbData, LPVOID lpvData)
{
	if (dinput_block_input)
	{
		memset(lpvData, 0, cbData);
		return DI_OK;
	}
	//Game lags severely with this hook, perhaps this is called many times?
	if (dinput_device_data_format7 == 1 && cbData == sizeof(DIJOYSTATE2))
	{
		p_ctrlr7->Unacquire();
		p_ctrlr7->SetDataFormat(&c_dfDIJoystick2);
		p_ctrlr7->Acquire();
		dinput_device_data_format7 = 2;
	}
	else if (dinput_device_data_format7 == 2 && cbData == sizeof(DIJOYSTATE))
	{
		p_ctrlr7->Unacquire();
		p_ctrlr7->SetDataFormat(&c_dfDIJoystick);
		p_ctrlr7->Acquire();
		dinput_device_data_format7 = 1;
	}

	if (!hasSetupDinputDeviceProperties7) setupDinputDeviceProperties7(pDev);
	//pCtrlr7->Poll();

	//wanderlust adventure cbData is 60, but sizeof(DIJOYSTATE) is 80

	DIJOYSTATE2 tempState;

	p_ctrlr7->GetDeviceState(cbData <= sizeof(DIJOYSTATE) ? sizeof(DIJOYSTATE) : sizeof(DIJOYSTATE2), &tempState);
	memcpy(lpvData, &tempState, cbData);
	return DI_OK;
}

//Buffered data
bool dinputBufferSetup = false;

HRESULT __stdcall Dinput_GetDeviceData_Hook(LPDIRECTINPUTDEVICE8 pDev, DWORD cbObjectData, LPDIDEVICEOBJECTDATA rgdod,
                                            LPDWORD pdwInOut, DWORD dwFlags)
{
	//Copy buffer size and data format
	if (!dinputBufferSetup)
	{
		dinputBufferSetup = true;

		//Data format
		{
			DIJOYSTATE state;
			if (pDev->GetDeviceState(sizeof(DIJOYSTATE), &state) == DIERR_INVALIDPARAM)
			{
				//Failed because it was DIJOYSTATE2, and couldn't copy it into the smaller DIJOYSTATE
				dinput_device->Unacquire();
				dinput_device->SetDataFormat(&c_dfDIJoystick2);
				dinput_device->Acquire();
				dinput_device_data_format = 2;
			}
			else
			{
				dinput_device->Unacquire();
				dinput_device->SetDataFormat(&c_dfDIJoystick);
				dinput_device->Acquire();
				dinput_device_data_format = 1;
			}
		}

		//Buffer size
		{
			DIPROPDWORD buffer_size;
			buffer_size.diph.dwSize = sizeof(DIPROPDWORD);
			buffer_size.diph.dwHeaderSize = sizeof(DIPROPHEADER);
			buffer_size.diph.dwHow = DIPH_DEVICE;
			buffer_size.diph.dwObj = 0;
			HRESULT gpRes = pDev->GetProperty(DIPROP_BUFFERSIZE, &buffer_size.diph);

			DIPROPDWORD buffer_size2;
			buffer_size2.diph.dwSize = sizeof(DIPROPDWORD);
			buffer_size2.diph.dwHeaderSize = sizeof(DIPROPHEADER);
			buffer_size2.diph.dwHow = DIPH_DEVICE;
			buffer_size2.diph.dwObj = 0;
			buffer_size2.dwData = buffer_size.dwData;

			dinput_device->Unacquire();
			HRESULT spRes = dinput_device->SetProperty(DIPROP_BUFFERSIZE, &buffer_size2.diph);
			dinput_device->Acquire();

			std::cout << "Get buffer size result = " << gpRes << ", set result = " << spRes << ", buffer size = " <<
				buffer_size.dwData << "\n";
		}
	}

	return dinput_device->GetDeviceData(cbObjectData, rgdod, pdwInOut, dwFlags);
}

HRESULT __stdcall Dinput_CreateDevice_Hook(IDirectInput8* pdin, REFGUID rguid,
                                           LPDIRECTINPUTDEVICE8A* lplpDirectInputDevice, LPUNKNOWN pUnkOuter)
{
	std::cout << "Dinput CreateDeviceHook called\n";

	if (dinput_block_input)
	{
		return DIERR_INVALIDPARAM; //pretend it failed
	}
	return pdin->CreateDevice(controller_guid, lplpDirectInputDevice, pUnkOuter);
}

HRESULT __stdcall Dinput7_CreateDevice_Hook(IDirectInputA* pdin, REFGUID rguid,
                                            LPDIRECTINPUTDEVICEA* lplpDirectInputDevice, LPUNKNOWN pUnkOuter)
{
	std::cout << "Dinput7 CreateDeviceHook called\n";

	if (dinput_block_input)
	{
		return DIERR_INVALIDPARAM; //pretend it failed
	}
	return pdin->CreateDevice(controller_guid, lplpDirectInputDevice, pUnkOuter);
}

void installDinputHook(void* EntryPoint, void* HookProc, string name)
{
	HOOK_TRACE_INFO hHook = {nullptr};

	NTSTATUS hookResult = LhInstallHook(
		EntryPoint,
		HookProc,
		nullptr,
		&hHook);

	if (!FAILED(hookResult))
	{
		ULONG ACLEntries[1] = {0};
		LhSetExclusiveACL(ACLEntries, 1, &hHook);

		std::cout << "Successfully installed dinput8 hook " << name << "\n";
	}
	else
	{
		std::cout << "Failed to install dinput8 hook " << name << " in module, NTSTATUS: " << hookResult << "\n";
	}
}

int compareGuids(const void* pGuid1, const void* pGuid2)
{
	GUID guid1 = *(GUID*)pGuid1;
	GUID guid2 = *(GUID*)pGuid2;

	unsigned long g1d1 = ((unsigned long *)&guid1)[0];
	unsigned long g1d2 = ((unsigned long *)&guid1)[1];
	unsigned long g1d3 = ((unsigned long *)&guid1)[2];
	unsigned long g1d4 = ((unsigned long *)&guid1)[3];

	unsigned long g2d1 = ((unsigned long *)&guid2)[0];
	unsigned long g2d2 = ((unsigned long *)&guid2)[1];
	unsigned long g2d3 = ((unsigned long *)&guid2)[2];
	unsigned long g2d4 = ((unsigned long *)&guid2)[3];

	if (g1d1 != g2d1)
	{
		return g2d1 - g1d1;
	}
	if (g1d2 != g2d2)
	{
		return g2d2 - g1d2;
	}
	if (g1d3 != g2d3)
	{
		return g2d3 - g1d3;
	}
	if (g1d4 != g2d4)
	{
		return g2d4 - g1d4;
	}
	return 0;
}

void installDinputHooks()
{
	//First 4 byte (32bit) or 8 bytes (64bit) of object with virtual functions will be a pointer to an array of pointers to the virtual functions
#ifdef X64
		using ptrSize = long long;//64 bit pointers
#else
	using ptrSize = int; //32 bit pointers
#endif


	LoadLibrary("dinput.dll");

	auto _DirectInputCreateA = GetProcAddress(GetModuleHandle("dinput.dll"), "DirectInputCreateA");
	typedef HRESULT (__stdcall * func_DirectInputCreate)(HINSTANCE hinst, DWORD dwVersion, LPDIRECTINPUTA* ppDI,
	                                                     LPUNKNOWN punkOuter);
	static func_DirectInputCreate DirectInputCreateA = (func_DirectInputCreate)_DirectInputCreateA;


	//extern HRESULT WINAPI DirectInputCreateEx(HINSTANCE hinst, DWORD dwVersion, REFIID riidltf, LPVOID *ppvOut, LPUNKNOWN punkOuter);

	auto _DirectInputCreateEx = GetProcAddress(GetModuleHandle("dinput.dll"), "DirectInputCreateEx");
	typedef HRESULT (__stdcall * func_DirectInputCreateEx)(HINSTANCE hinst, DWORD dwVersion, REFIID riidltf,
	                                                       LPVOID* ppvOut, LPUNKNOWN punkOuter);
	static func_DirectInputCreateEx DirectInputCreateEx = (func_DirectInputCreateEx)_DirectInputCreateEx;

	//DirectInputCreateA(DllHandle, 0x0700, &pDinput7, NULL);
	HRESULT r0 = DirectInputCreateEx(DllHandle, 0x0700, IID_IDirectInput7, (void**)&p_dinput7, nullptr);

	p_dinput7->CreateDevice(controller_guid, &p_ctrlr7, nullptr);

	HRESULT r1 = p_ctrlr7->SetDataFormat(GetdfDIJoystick());
	dinput_device_data_format7 = 1;
	HRESULT r2 = p_ctrlr7->SetCooperativeLevel(hWnd, DISCL_BACKGROUND | DISCL_NONEXCLUSIVE);

	HRESULT r3 = p_ctrlr7->EnumObjects(&DIEnumDeviceObjectsCallback7, p_ctrlr7, DIDFT_AXIS);
	HRESULT r4 = p_ctrlr7->Acquire();

	std::cout << "r0..4 = " << r0 << ", " << r1 << ", " << r2 << ", " << r3 << ", " << r4 << "\n";

	/*std::cout << "r1..4 = " << r1 << ", " << r2 << ", " << r3 << ", " << r4 << "\n";

	for (int aisudhfio = 0; aisudhfio < 15; aisudhfio++)
	{
		DIJOYSTATE asdf;
		pCtrlr7->GetDeviceState(sizeof(DIJOYSTATE), &asdf);
		std::cout << "p7 lx=" << asdf.lX << "\n";
		std::cout << "p7 =" << asdf.lY << "\n";
		std::cout << "p7 =" << asdf.lZ << "\n";
		std::cout << "p7 =" << asdf.lRx << "\n";
		std::cout << "p7 =" << asdf.lRy << "\n";
		std::cout << "p7 =" << asdf.lRz << "\n";
		Sleep(1500);
	}*/


	//https://kaisar-haque.blogspot.com/2008/07/c-accessing-virtual-table.html
	ptrSize* vptr_device = *(ptrSize**)dinput_device;
	ptrSize* vptr_dinput = *(ptrSize**)p_dinput;

	ptrSize* vptr_device7 = *(ptrSize**)p_ctrlr7;
	ptrSize* vptr_dinput7 = *(ptrSize**)p_dinput7;

	using GetDeviceStateFunc = HRESULT(__stdcall *)(DWORD, LPVOID);
	GetDeviceStateFunc GetDeviceStatePointer = (GetDeviceStateFunc)vptr_device[9];
	installDinputHook(GetDeviceStatePointer, Dinput_GetDeviceState_Hook, "GetDeviceState");

	/*//using GetDeviceStateFunc = HRESULT(__stdcall *)(DWORD, LPVOID);
	GetDeviceStateFunc GetDeviceStatePointer7 = (GetDeviceStateFunc)vptr_device7[9];
	installDinputHook(GetDeviceStatePointer7, Dinput_GetDeviceState_Hook7, "GetDeviceState (dinput 7)");*/
	installDinputHook((void*)(vptr_device7[9]), Dinput_GetDeviceState_Hook7, "GetDeviceState (dinput 7)");


	using GetDeviceDataFunc = HRESULT(__stdcall *)(DWORD, LPDIDEVICEOBJECTDATA, LPDWORD, DWORD);
	GetDeviceDataFunc GetDeviceDataPointer = (GetDeviceDataFunc)vptr_device[10];
	installDinputHook(GetDeviceDataPointer, Dinput_GetDeviceData_Hook, "GetDeviceData");

	//(needs its own function signature)
	//GetDeviceDataFunc GetDeviceDataPointer7 = (GetDeviceDataFunc)vptr_device7[10];
	//installDinputHook(GetDeviceDataPointer7, Dinput_GetDeviceData_Hook, "GetDeviceData (dinput 7)");

	using CreateDeviceFunc = HRESULT(__stdcall *)(REFGUID rguid, LPDIRECTINPUTDEVICE8A* lplpDirectInputDevice,
	                                              LPUNKNOWN pUnkOuter);
	CreateDeviceFunc CreateDevicePointer = (CreateDeviceFunc)vptr_dinput[3];
	installDinputHook(CreateDevicePointer, Dinput_CreateDevice_Hook, "CreateDevice");

	/*using CreateDeviceFunc7 = HRESULT(__stdcall *)(REFGUID rguid, LPDIRECTINPUTDEVICEA* lplpDirectInputDevice, LPUNKNOWN pUnkOuter);
	CreateDeviceFunc7 CreateDevicePointer7 = (CreateDeviceFunc7)vptr_dinput7[3];
	installDinputHook(CreateDevicePointer7, Dinput7_CreateDevice_Hook, "CreateDevice (dinput 7)");*/
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
	while (ShowCursor(FALSE) >= -10 && i++ < 20);
	SetCursor(nullptr);

	if (inRemoteInfo->UserDataSize == 1024)
	{
		//Get UserData
		BYTE* data = inRemoteInfo->UserData;

		string ipcChannelName = string(reinterpret_cast<char*>(data), 256);
		_ipcChannelNameRead = ipcChannelName;
		std::cout << "Received IPC channel read: " << ipcChannelName << "\n";

		string ipcChannelNameWrite = string(reinterpret_cast<char*>(data + 256), 256);
		_ipcChannelNameWrite = ipcChannelNameWrite;
		std::cout << "Received IPC channel write: " << ipcChannelNameWrite << "\n";

		hWnd = (HWND)bytesToInt(data + 512);
		std::cout << "Received hWnd: " << hWnd << "\n";

		controller_index = bytesToInt(data + 512 + 4);
		std::cout << "Received controller index: " << controller_index << "\n";

		allowed_mouse_handle = (HANDLE)bytesToInt(data + 512 + 8);
		std::cout << "Allowed mouse handle: " << allowed_mouse_handle << "\n";

		update_absolute_flag_in_mouse_message = *(data + 512 + 12) == 1;
		std::cout << "Update absolute flag in mouse message: " << update_absolute_flag_in_mouse_message << "\n";

		BYTE* _p = data + 512 + 12 + 1;

#define NEXT_BOOL(a) bool a = *(_p++) == 1;
		NEXT_BOOL(HookGetCursorPos)
		NEXT_BOOL(HookGetForegroundWindow)
		NEXT_BOOL(HookGetAsyncKeyState)
		NEXT_BOOL(HookGetKeyState)
		NEXT_BOOL(HookCallWindowProcW)
		NEXT_BOOL(HookRegisterRawInputDevices)
		NEXT_BOOL(HookSetCursorPos)
		NEXT_BOOL(HookXInput)
		NEXT_BOOL(useLegacyInput)
		NEXT_BOOL(hookMouseVisibility)
		NEXT_BOOL(hookDinput)
		HookDinput = hookDinput;
#undef NEXT_BOOL

#define NAME_OF_VAR(x) #x
#define PRINT_BOOL(b) std::cout << NAME_OF_VAR(b) << " = " << (b == TRUE ? "TRUE" : "FALSE") << "\n";

		PRINT_BOOL(HookGetCursorPos)
		PRINT_BOOL(HookGetForegroundWindow)
		PRINT_BOOL(HookGetAsyncKeyState)
		PRINT_BOOL(HookGetKeyState)
		PRINT_BOOL(HookCallWindowProcW)
		PRINT_BOOL(HookRegisterRawInputDevices)
		PRINT_BOOL(HookSetCursorPos)
		PRINT_BOOL(HookXInput)
		PRINT_BOOL(useLegacyInput)
		PRINT_BOOL(hookMouseVisibility)
		PRINT_BOOL(hookDinput)

#undef NAME_OF_VAR
#undef PRINT_BOOL

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

		if (HookGetCursorPos) installHook(TEXT("user32"), "GetCursorPos", GetCursorPos_Hook);
		if (HookGetAsyncKeyState) installHook(TEXT("user32"), "GetAsyncKeyState", GetAsyncKeyState_Hook);
		if (HookGetKeyState) installHook(TEXT("user32"), "GetKeyState", GetKeyState_Hook);
		if (HookSetCursorPos) installHook(TEXT("user32"), "SetCursorPos", SetCursorPos_Hook);
		//if (filterRawInput)						installHook(TEXT("user32"), "RegisterRawInputDevices",	RegisterRawInputDevices_Hook);

		filter_raw_input = HookRegisterRawInputDevices;
		filter_mouse_messages = HookCallWindowProcW;
		//if (filterRawInput || filterMouseMessages)	installHook(TEXT("user32"), "CallWindowProcW",			CallWindowProc_Hook);

		if (filter_raw_input || filter_mouse_messages || enable_legacy_input)
		{
			installHook(TEXT("user32"), "GetMessageA", GetMessageA_Hook);
			installHook(TEXT("user32"), "GetMessageW", GetMessageW_Hook);

			installHook(TEXT("user32"), "PeekMessageA", PeekMessageA_Hook);
			installHook(TEXT("user32"), "PeekMessageW", PeekMessageW_Hook);
		}

		if (hookDinput)
		{
			dinput_device = nullptr;

			HRESULT dinput_ret = DirectInput8Create(DllHandle, DIRECTINPUT_VERSION, IID_IDirectInput8,
			                                        (void**)&(p_dinput), nullptr);

			if (dinput_ret != DI_OK)
			{
				std::cerr << "Fail DirectInput8Create, dinput_ret=" << dinput_ret << endl;
			}
			else
			{
				std::cout << "Succeed dDirectInput8Create\n";
				dinputGuids_i = 0;
				p_dinput->EnumDevices(DI8DEVCLASS_ALL, DIEnumDevicesCallback, nullptr, DIEDFL_ALLDEVICES);
				//std::qsort(dinputGuids, maxDinputDevices, sizeof(GUID), compareGuids);

				if (controller_index == 0)
				{
					dinput_block_input = true;
					controller_guid = dinput_guids[controller_index - 1];
					if (DI_OK == p_dinput->CreateDevice(controller_guid, &dinput_device, nullptr))
					{
						//installDinputHooks();
					}
				}
				else if (!(controller_index <= max_dinput_devices && controller_index <= dinputGuids_i))
				{
					std::cerr << "Not selecting dinput controller because controllerIndex out of range" << endl;
					MessageBox(nullptr, "Not selecting dinput controller because controllerIndex out of range", nullptr,
					           MB_OK);
				}
				else
				{
					controller_guid = dinput_guids[controller_index - 1];
					std::cout << "cg8=" << controller_guid.Data1 << "\n";
					HRESULT cdRes = p_dinput->CreateDevice(controller_guid, &dinput_device, nullptr);

					if (cdRes != DI_OK)
					{
						std::cerr << "dinput create device error: " << cdRes << endl;
					}
					else
					{
						//Dinput8 hook
						//installDinputHooks();

						dinput_device->SetCooperativeLevel(hWnd, DISCL_BACKGROUND | DISCL_NONEXCLUSIVE);

						dinput_device->SetDataFormat(&c_dfDIJoystick2);
						dinput_device_data_format = 2;

						DIDEVCAPS caps;
						caps.dwSize = sizeof(DIDEVCAPS);
						HRESULT gcRes = dinput_device->GetCapabilities(&caps);

						std::cout << "dinput device number of buttons = " << caps.dwButtons << "\n";
						std::cout << "dinput device number of axes = " << caps.dwAxes << "\n";

						dinput_device->EnumObjects(&DIEnumDeviceObjectsCallback, dinput_device, DIDFT_AXIS);

						HRESULT aquireResult = dinput_device->Acquire();

						if (aquireResult == DI_OK)
							std::cout << "Successfully aquired dinput device\n";
						else
							std::cout << "Failed to aquired dinput device\n";
					}
				}
			}
		}

		//Hook XInput dll
		if (HookXInput)
		{
			LPCSTR xinputNames[] = {
				"xinput1_3.dll", "xinput1_4.dll", "xinput1_2.dll", "xinput1_1.dll", "xinput9_1_0.dll"
			};

			for (int xi = 0; xi < 5; xi++)
			{
				if (GetModuleHandleA(xinputNames[xi]) != nullptr)
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

		//Start named pipe client
		startPipeListen();
	}
	else
	{
		std::cout << "Failed getting user data\nExpected size 1024, Received " << (inRemoteInfo->UserDataSize) << "\n";
	}
}
