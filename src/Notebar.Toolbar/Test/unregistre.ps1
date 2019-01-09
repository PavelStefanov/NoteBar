# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

& $RegasmPath /u ../bin/Debug/Notebar.Toolbar.dll

Stop-Process -ProcessName explorer