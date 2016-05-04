@echo off

@echo off
cd %~dp0

SETLOCAL
SET NUGET_VERSION=latest
SET CACHED_NUGET=%LocalAppData%\NuGet\nuget.%NUGET_VERSION%.exe

IF EXIST "%CACHED_NUGET%" goto copynuget
echo Downloading latest version of NuGet.exe...

IF NOT EXIST "%LocalAppData%\NuGet" md "%LocalAppData%\NuGet"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/%NUGET_VERSION%/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy "%CACHED_NUGET%" .nuget\nuget.exe > nul

:restore
.nuget\NuGet.exe restore

:build
"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" uFastly.sln /p:configuration=release /p:VisualStudioVersion=14.0

:package
IF NOT EXIST "pkg" md "pkg"
.nuget\NuGet.exe pack FastlyNet\FastlyNet.csproj -OutputDirectory "pkg" -Prop Configuration=Release