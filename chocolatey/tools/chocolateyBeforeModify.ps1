#Requires -RunAsAdministrator

$ErrorActionPreference = 'Stop';

$NoteBarDir = "$env:Programfiles\NoteBar"
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
if (Test-Path -Path $NoteBarDir) {
    Remove-Item -Path $NoteBarDir -Recurse -Force
}