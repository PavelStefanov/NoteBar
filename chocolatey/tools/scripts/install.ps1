#Requires -RunAsAdministrator

$NoteBarDir = "$env:Programfiles\NoteBar"

# Copy src
New-Item -ItemType Directory -Path $NoteBarDir
Get-ChildItem -Path "src" | Copy-Item -Destination $NoteBarDir
Remove-Item -Path "src" -Recurse -Force

# Add NoteBar to PATH
[Environment]::SetEnvironmentVariable("Path", "$env:Path;$NoteBarDir", [System.EnvironmentVariableTarget]::Machine)

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Registre NoteBar deskband
& $RegasmPath /codebase "$NoteBarDir\NoteBar.Toolbar.dll"