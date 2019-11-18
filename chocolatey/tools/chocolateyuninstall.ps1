#Requires -RunAsAdministrator

$ErrorActionPreference = 'Stop';

$NoteBarDir = "$env:Programfiles\NoteBar"
$NoteBarAppData = "$env:APPDATA\NoteBar"

# Delete NoteBar AppData directory
if (Test-Path -Path $NoteBarAppData) {
    Remove-Item -Path $NoteBarAppData -Recurse -Force
} 

# Delete EventSource of NoteBar logging
if ([System.Diagnostics.EventLog]::SourceExists("NoteBar")) {
    [System.Diagnostics.EventLog]::DeleteEventSource("NoteBar")
}

# Delete notebar from PATH
$Path = [Environment]::GetEnvironmentVariable("Path")
$NewPath = ($Path.Split(";") | Where-Object { $_ -ne $NoteBarDir }) -join ";"
[Environment]::SetEnvironmentVariable("Path", 
    $NewPath, [System.EnvironmentVariableTarget]::Machine)