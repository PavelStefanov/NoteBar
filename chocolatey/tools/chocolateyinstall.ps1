#Requires -RunAsAdministrator

$ErrorActionPreference = 'Stop';

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

Get-ChocolateyUnzip "$toolsDir\src.zip" "$toolsDir\src"

$NoteBarDir = "$env:Programfiles\NoteBar"

# Create or clear NoteBar directory
if (!(Test-Path -Path $NoteBarDir)) {
    New-Item -ItemType Directory -Path $NoteBarDir    
} 
# If Notebar's already installed. Hack. Next it'll be unnecessary
else {
    $NoteBarDll = "$NoteBarDir\NoteBar.Toolbar.dll"

    # Unregister NoteBar's toolbar
    if (Test-Path -Path $NoteBarDll) {
        # Get regasm path
        $dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
        $RegasmPath = "$dotnetPath\RegAsm.exe"

        # Unregistre NoteBar's deskband
        & $RegasmPath /u $NoteBarDll
        Stop-Process -ProcessName explorer
        Start-Sleep -s 2
    }

    # Delete src
    Remove-Item -Path "$NoteBarDir\*" -Recurse -Force
}

# Copy src
Get-ChildItem -Path "$toolsDir\src" | Copy-Item -Destination $NoteBarDir

# Add NoteBar to PATH
$Path = [Environment]::GetEnvironmentVariable("Path")
if (!($Path.Split(";") -contains $NoteBarDir)) {
    [Environment]::SetEnvironmentVariable("Path", 
        "$env:Path;$NoteBarDir", [System.EnvironmentVariableTarget]::Machine)
}

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