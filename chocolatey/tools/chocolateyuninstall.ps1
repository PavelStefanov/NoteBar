$ErrorActionPreference = 'Stop';
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

Start-ChocolateyProcessAsAdmin "& `'$toolsDir\scripts\uninstall.ps1`'"