#Requires -RunAsAdministrator

$NoteBarDir = "$env:Programfiles\NoteBar"

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Unregistre notebar deskband
& $RegasmPath /u "$NoteBarDir\NoteBar.Toolbar.dll"
Stop-Process -ProcessName explorer
Start-Sleep -s 2

# Delete src
Remove-Item -Path $NoteBarDir -Recurse -Force

# Delete notebar from PATH
$Path=[Environment]::GetEnvironmentVariable("Path")
$NewPath = ($Path.Split(";") | Where-Object { $_ -ne $NoteBarDir }) -join ";"
[Environment]::SetEnvironmentVariable("Path", $NewPath, [System.EnvironmentVariableTarget]::Machine)