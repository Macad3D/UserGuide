@echo off

rem Find MSBuild
setlocal enabledelayedexpansion
set VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

for /f "usebackq tokens=*" %%i in (`%VSWHERE% -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\msbuild.exe`) do (
  set MSBUILDPATH=%%i
)

if not exist "%MSBUILDPATH%" (
	echo Cannot find msbuild.exe, please install VisualStudio with MSBuild.
	pause
	exit /b 1
)

"%MSBUILDPATH%" /p:Configuration=Release tool\Macad.UserGuide.sln /t:restore,build /m /verbosity:minimal
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%

"%~dp0\tool\bin\Release\Macad.UserGuide.exe" %*
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
