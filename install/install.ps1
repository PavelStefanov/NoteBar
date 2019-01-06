#Requires -RunAsAdministrator

$NotebarDir = "$env:SystemDrive\Program Files\Notebar\"

# Copy src
New-Item -ItemType Directory -Path $NotebarDir
Get-ChildItem -Path "src\" | Copy-Item -Destination $NotebarDir

# Add Notebar to PATH
$Enviroment = "Registry::HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment"
$Path=(Get-ItemProperty -Path $Enviroment -Name Path).Path
Set-ItemProperty -Path $Enviroment -Name PATH -Value ($Path += ";$NotebarDir")

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Registre Notebar deskband
& $RegasmPath /codebase "$($NotebarDir)Notebar.Toolbar.dll"