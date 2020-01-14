Mount and unmount ISO files via CMDline in Windows 10

Useful with [Launchbox](https://www.launchbox-app.com/)

This is a pretty simple program and there isn't much

The commands you can submit are
```
-mount ISOPATH
-unmount ISOPATH
-lbox ISOPATH APPPATH APPARGUMENTS
```

 Examples:
```
 mountme -mount "c:\some folder\file.iso"
 mountme -unmount "c:\some folder\file.iso"
 mountme -lbox "c:\some folder\file.iso" "c:\some folder\file.exe"
 mountme -lbox "c:\some folder\file.iso" "c:\some folder\file.exe" "-n -i -g"
```

Compiled EXE in the [bin/Release](https://github.com/go2tom42/C-Sharp/tree/master/MountMe/bin/Release) folder if needed