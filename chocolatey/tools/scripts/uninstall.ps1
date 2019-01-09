#Requires -RunAsAdministrator

$NotebarDir = "$env:Programfiles\Notebar"

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Unregistre notebar deskband
& $RegasmPath /u "$NotebarDir\Notebar.Toolbar.dll"
Stop-Process -ProcessName explorer
Start-Sleep -s 2

# Delete src
Remove-Item -Path $NotebarDir -Recurse -Force

# Delete notebar from PATH
$Path=[Environment]::GetEnvironmentVariable("Path")
$NewPath = ($Path.Split(";") | Where-Object { $_ -ne $NotebarDir }) -join ";"
[Environment]::SetEnvironmentVariable("Path", $NewPath, [System.EnvironmentVariableTarget]::Machine)