# UniversalSplitScreen
Split screen multiplayer for any game with multiple keyboards, mice and controllers.
https://universalsplitscreen.github.io/

### Compiling notes
Compile UniversalSplitScreen in x86.
Compile SourceEngineUnlocker in x86, and copy SourceEngineUnlocker.dll to the UniversalSplitScreen folder.
Compile InjectorLoader in x86 and x64. Copy IJx86.exe and IJx64.exe to the folder.
Compile HooksCPP in x86 and x64. Copy HooksCPP32.dll and HooksCPP64.dll to the folder.
Compile FindWindowHook in x86 and x64. Copy FindWindowHook32.dll and FindWindowHook64.dll to the folder.
The post-build options in the projects should automatically rename and copy as necessary.
You can use Batch build in Visual Studio to compile all the projects at once.
