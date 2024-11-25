cd /d %~dp0

@REM This script is used to publish the project to a folder and compress it to a 7z file.
@REM You should have 7z installed and added to PATH.
@REM You should prepare the bin file ffmpeg.exe and ffplay.exe in publish folder.

del /s /q ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\TiktokLiveRec.exe
dotnet publish ..\src\TiktokLiveRec.Windows\TiktokLiveRec.Windows.csproj -c Release -p:PublishProfile=FolderProfile
rd /s /q ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\..\TiktokLiveRec\
mkdir ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\..\TiktokLiveRec\
rd /s /q ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\downloads\
xcopy /s /e /y ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\* ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\..\TiktokLiveRec\
del /s /q publish.7z
7z a publish.7z ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\..\TiktokLiveRec\ -t7z -mx=5 -mf=BCJ2 -r -y
rd /s /q ..\src\TiktokLiveRec.Windows\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\..\TiktokLiveRec\
for /f "usebackq delims=" %%i in (`powershell -NoLogo -NoProfile -Command "Get-Content '..\src\TiktokLiveRec.Windows\TiktokLiveRec.Windows.csproj' | Select-String -Pattern '<AssemblyVersion>(.*?)</AssemblyVersion>' | ForEach-Object { $_.Matches.Groups[1].Value }"`) do @set version=%%i
del /s /q TiktokLiveRec_v%version%_win64.7z
rename publish.7z TiktokLiveRec_v%version%_win64.7z

@pause
