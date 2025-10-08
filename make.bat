@echo off

FOR /F "tokens=* USEBACKQ" %%F IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) DO (
SET msbuild="%%F"
)
ECHO %msbuild%

@%msbuild% sergiye.StatusPageIoApi.sln /t:restore /p:RestorePackagesConfig=true
@%msbuild% sergiye.StatusPageIoApi.sln /t:Rebuild /p:DebugType=None /p:Configuration=Release

rem dotnet build sergiye.StatusPageIoApi.sln -c Release -p:DebugType=None -p:Platform="Any CPU"
rem dotnet pack sergiye.StatusPageIoApi.sln -c Release -p:DebugType=None -p:Platform="Any CPU"

if errorlevel 1 goto error

goto exit
:error
pause
:exit
