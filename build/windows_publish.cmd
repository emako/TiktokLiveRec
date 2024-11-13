cd /d %~dp0

cd ..\
dotnet publish src\TiktokLiveRec.csproj -c Release -p:PublishProfile=FolderProfile

@pause
