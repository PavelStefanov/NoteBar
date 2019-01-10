# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

& $RegasmPath /codebase ../bin/Debug/NoteBar.Toolbar.dll