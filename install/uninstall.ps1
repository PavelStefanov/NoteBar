#Requires -RunAsAdministrator

$NotebarDir = "$env:SystemDrive\Program Files\Notebar\"

# Get regasm path
$dotnetPath = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$RegasmPath = "$dotnetPath\RegAsm.exe"

# Unregistre notebar deskband
& $RegasmPath /u "$($NotebarDir)Notebar.Toolbar.dll"
Stop-Process -ProcessName explorer
Start-Sleep -s 2

# Delete src
Remove-Item -Path $NotebarDir -Recurse -Force

# Delete notebar from PATH
$Enviroment = "Registry::HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment"
$Path=(Get-ItemProperty -Path $Enviroment -Name Path).Path
$NewPath = ($Path.Split(";") | Where-Object { $_ -ne $NotebarDir }) -join ";"
Set-ItemProperty -Path $Enviroment -Name PATH -Value $NewPath