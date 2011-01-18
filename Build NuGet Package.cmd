@echo off
if not exist Binaries mkdir Binaries
cd Source
..\Util\nuget.exe p Nito.ConnectedProperties.nuspec -o ..\Binaries
pause