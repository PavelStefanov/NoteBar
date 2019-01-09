#Requires -RunAsAdministrator

$NotebarDir = "$env:Programfiles\Notebar"

# Copy src
New-Item -ItemType Directory -Path $NotebarDir
Get-ChildItem -Path "src" | Copy-Item -Destination $NotebarDir
Remove-Item -Path "src" -Recurse -Force

# Add Notebar to PATH
[Environment]::SetEnvironmentVariable("Path", "$env:Path;$NotebarDir", [System.EnvironmentVariableTarget]::Machine)

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Registre Notebar deskband
& $RegasmPath /codebase "$NotebarDir\Notebar.Toolbar.dll"