# NoteBar: Windows taskbar toolbar indicators

NoteBar is small indicators for Windows taskbar those can display a colored dot or a custom icon.

NoteBar is a clone of [AnyBar](https://github.com/tonsky/AnyBar)

<img src="assets/screenshot.png?raw=true" />

## Install

You can install using [Chocolatey](https://chocolatey.org/)

```powershell
choco install notebar
```

## Usage

First you must launch NoteBar. To run NoteBar right-click the taskbar, in the pop-up menu that appears, click Toolbars and then click NoteBar.

Next you can run indicator by execute in powershell

```powershell
notebar
```
*If NoteBar is not launched you'll see a dialog box to add NoteBar on your taskbar.*

NoteBar is controlled via a UDP port (1738 by default). Send it a message and it will change a color:
```powershell
$Message = [System.Text.Encoding]::UTF8.GetBytes("black");
(New-Object System.Net.Sockets.UDPClient).Send($Message, $Message.length, "localhost", 1738)
```

The following default commands change the style of the dot:

<img src="src/NoteBar.Core/Icons/Resources/white.png?raw=true" width=16 /> `white`  
<img src="src/NoteBar.Core/Icons/Resources/red.png?raw=true" width=16 /> `red`  
<img src="src/NoteBar.Core/Icons/Resources/orange.png?raw=true" width=16 /> `orange`  
<img src="src/NoteBar.Core/Icons/Resources/yellow.png?raw=true" width=16 /> `yellow`  
<img src="src/NoteBar.Core/Icons/Resources/green.png?raw=true" width=16 /> `green`  
<img src="src/NoteBar.Core/Icons/Resources/cyan.png?raw=true" width=16 /> `cyan`  
<img src="src/NoteBar.Core/Icons/Resources/blue.png?raw=true" width=16 /> `blue`  
<img src="src/NoteBar.Core/Icons/Resources/purple.png?raw=true" width=16 /> `purple`  
<img src="src/NoteBar.Core/Icons/Resources/black.png?raw=true" width=16 /> `black`  
<img src="src/NoteBar.Core/Icons/Resources/question.png?raw=true" width=16 /> `question`  
<img src="src/NoteBar.Core/Icons/Resources/exclamation.png?raw=true" width=16 /> `exclamation`  

And one special command forces NoteBar to quit: `quit`

## Running multiple indicators

You can run several indicators as long as they listen on different ports. Use `-p` or `--port` command line argument to change port:

```powershell
notebar -p 1738
notebar -p 1739
notebar -p 1740
```

## Custom icons

NoteBar can detect and use local custom images stored in the `%APPDATA%\NoteBar` directory. E.g. if you have `%APPDATA%\NoteBar\square.png` icon, send `square` and it will be displayed. Icons should be 16×16 pixels.
