---
uid: 46793f6d-da23-48ea-913a-186727062a80
title: Running on other Platforms
---
Macad|3D has been developed for the Windows operating system on x64 architecture. However, it is possible to run the program on other operating systems and other architectures like Arm. This requires an emulation that provides the program with the required operating environment. Here are some tips to increase the experience on the respective systems.

## Windows on Arm

For Windows on Arm an emulation is executed in the operating system itself, this should run Macad|3D as usual. Since it is still an x64 application, you need a 64bit operating system.

## MacOS

Operation on MacOS platform has been successfully tested using the emulation software [Parallels Desktop](https://www.parallels.com/). Furthermore, the emulation solutions [Crossover](https://www.codeweavers.com/crossover) and [Wine](https://www.winehq.org/) are available, which have already been successfully tested under Linux. For more details, see the next chapter.

For the installation of Wine on MacOS see [here](https://gitlab.winehq.org/wine/wine/-/wikis/MacOS).

## Linux

Operation on Linux was tested using the commercial emulation software [Crossover](https://www.codeweavers.com/crossover) and the free version [Wine](https://www.winehq.org/).

Wine is available in most distributions via the respective app manager, but the software version may already be older and therefore still contain errors, which usually show up in the UI rendering. The operation of Macad|3D was tested on Ubuntu 23. If UI/display errors occur, manual installation of the latest version is recommended. Wine itself provides instructions for this.

For the installation of Wine on Linux see [WineHQ Installation and Configuration](https://gitlab.winehq.org/wine/wine/-/wikis/Wine-Installation-and-Configuration).

Please make sure you also have installed [Winetricks](https://gitlab.winehq.org/wine/wine/-/wikis/Winetricks) and the [Corefonts](https://linuxconfig.org/configuring-wine-with-winetricks).

### Install Macad|3D using Wine
Download the latest version from the [Macad|3D Website](https://macad3d.net). Start a shell prompt and navigate to the directory containing the downloaded installer executable and run the executable using Wine:
```bash
cd /path/to/downloaded/installer
wine Macad3D_4.0_Setup.exe
```
Replace `Macad3D_4.0_Setup.exe` with the actual name of the downloaded installer version.

Follow the on-screen instructions for the installer.

To keep the display of the application window as close as possible to the original, it is recommended to disable the setting _Allow the window manager to decorate the windows_.
This can be done in _Wine configuration_ tool globally or in a special setup for Macad|3D. 

After the installation succeded, start the Wine configuration tool:
executable using Wine:
```bash
winecfg
```

Create a new profile, selected the file `Macad.exe` inside the installation directory (default: `C:\Program Files\Macad3D`). On the _Graphics_ tab, ensure that the checkbox _Allow the window manager to decorate the windows_ is unset, and save/apply these settings.

^![Create Profile in _WineCfg_](WineConfigure.apng)




