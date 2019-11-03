#Requires -RunAsAdministrator

$ErrorActionPreference = 'Stop';

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

Get-ChocolateyUnzip "$toolsDir\src.zip" "$toolsDir\src"

$NoteBarDir = "$env:Programfiles\NoteBar"

# Copy src
if (!(Test-Path -Path $NoteBarDir )) {
    New-Item -ItemType Directory -Path $NoteBarDir    
}
Get-ChildItem -Path "src" | Copy-Item -Destination $NoteBarDir

# Add NoteBar to PATH
[Environment]::SetEnvironmentVariable("Path", "$env:Path;$NoteBarDir", [System.EnvironmentVariableTarget]::Machine)

# Add EventSource for NoteBar logging
if (!([System.Diagnostics.EventLog]::SourceExists("NoteBar"))) {
    [System.Diagnostics.EventLog]::CreateEventSource("NoteBar", "Application")    
}

# Add NoteBar folder for local icons
$NoteBarAppDataDir = "$env:APPDATA\NoteBar"
if (!(Test-Path -Path $NoteBarAppDataDir )) {
    New-Item -ItemType Directory -Path $NoteBarAppDataDir
}

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Registre NoteBar deskband
& $RegasmPath /codebase "$NoteBarDir\NoteBar.Toolbar.dll"

Remove-Item -Path "$toolsDir\src" -Recurse -Force