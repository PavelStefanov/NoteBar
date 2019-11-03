#Requires -RunAsAdministrator

$ErrorActionPreference = 'Stop';

$NoteBarDir = "$env:Programfiles\NoteBar"

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Unregistre notebar deskband
& $RegasmPath /u "$NoteBarDir\NoteBar.Toolbar.dll"
Stop-Process -ProcessName explorer
Start-Sleep -s 2

# Delete NoteBar AppData folder
Remove-Item -Path "$env:APPDATA\NoteBar" -Recurse -Force

# Delete EventSource of NoteBar logging
[System.Diagnostics.EventLog]::DeleteEventSource("NoteBar")

# Delete notebar from PATH
$Path = [Environment]::GetEnvironmentVariable("Path")
$NewPath = ($Path.Split(";") | Where-Object { $_ -ne $NoteBarDir }) -join ";"
[Environment]::SetEnvironmentVariable("Path", $NewPath, [System.EnvironmentVariableTarget]::Machine)

# Delete src
Remove-Item -Path $NoteBarDir -Recurse -Force