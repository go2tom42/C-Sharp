Mount and unmount ISO files via CMDline in Windows 10

Useful with [Launchbox](https://www.launchbox-app.com/)

This is a pretty simple program and there isn't much

The commands you can submit are
```
-mount ISOPATH
-unmount ISOPATH
-lbox ISOPATH APPPATH APPARGUMENTS
If you use %DRIVE% or %DRIVE:% (note the colon) in APPARGUMENTS it's replaced with the drive letter of mounted ISO
```

 Examples:
```
 mountme -mount "c:\some folder\file.iso"
 mountme -unmount "c:\some folder\file.iso"
 mountme -lbox "c:\some folder\file.iso" "c:\some folder\file.exe"
 mountme -lbox "c:\some folder\file.iso" "c:\some folder\file.exe" "-d %DRIVE% -Fullscreen"
          After mounted App run like "c:\some folder\file.exe" "-d G -Fullscreen"
 
 mountme -lbox "games\Game Name\file.iso" "\emulators\emulator\file.exe" "%DRIVE:%\PS3_GAME\USRDIR\EBOOT.BIN"
          After mounted App run like "e:\Launchbox\emulators\emulator\file.exe" "G:\PS3_GAME\USRDIR\EBOOT.BIN"
```

Relative paths will work in LaunchBox mode (-lbox) **IF** LaunchBox.exe (or BigBox.exe) is running

Relative paths assume your launchbox folder where LaunchBox is running from as it starting point

Compiled EXE in the [bin/Release](https://github.com/go2tom42/C-Sharp/tree/master/MountMe/bin/Release) folder if needed
