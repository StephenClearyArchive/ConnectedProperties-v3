@echo off
if not exist ..\Binaries mkdir ..\Binaries
devenv Nito.ConnectedProperties.sln /rebuild Release
..\Util\nuget.exe pack Nito.ConnectedProperties.old.nuspec -o ..\Binaries
..\Util\nuget.exe pack -sym Nito.ConnectedProperties.nuspec -o ..\Binaries
