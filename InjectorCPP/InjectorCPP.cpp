// InjectorCPP.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <cstring>
#include <easyhook.h>


#include <iostream>
#include <fstream>
using namespace std;

struct UserData
{
	HWND hWnd;
	char ipcChannelName[256];//EasyHook.RemoteHooking.GenerateName will be between 20 and 29 characters
	int pipeHandle;
};

extern "C" __declspec(dllexport) int Inject(int pid, WCHAR* injectionDllPath, HWND hWnd, char* ipcChannelName, int pipeHandle)
{
	ofstream myfile;
	myfile.open("C:\\Projects\\UniversalSplitScreen\\UniversalSplitScreen\\bin\\x86\\Debug\\InjectorCPP_Output.txt");
	std::string ipcChannelName2(ipcChannelName);
	myfile << ipcChannelName2 << "\n";
	myfile << ipcChannelName << "\n";
	myfile << &ipcChannelName;
	myfile.close();
	
	UserData* data = new UserData();
	data->hWnd = hWnd;
	strcpy_s(data->ipcChannelName, ipcChannelName);
	data->pipeHandle = pipeHandle;

	NTSTATUS nt = RhInjectLibrary(
		pid,
		0,
		EASYHOOK_INJECT_DEFAULT,
		injectionDllPath,
		NULL,
		data,
		sizeof(UserData)
	);

	return nt;//NTSTATUS: 32-bit
}