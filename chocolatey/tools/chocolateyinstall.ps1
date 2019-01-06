$ErrorActionPreference = 'Stop';
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

Get-ChocolateyUnzip "$toolsDir\src.zip" $toolsDir\src\

Start-ChocolateyProcessAsAdmin "& `'$toolsDir\scripts\install.ps1`'" -WorkingDirectory $toolsDir