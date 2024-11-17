cd /d %~dp0

cd ..\
dotnet publish src\TiktokLiveRec.csproj -c Release -p:PublishProfile=FolderProfile
rd /s /q ..\src\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\downloads\
7z a publish.7z ..\src\bin\x64\Release\net9.0-windows10.0.26100.0\publish\win-x64\* -t7z -mx=5 -mf=BCJ2 -r -y

@pause
