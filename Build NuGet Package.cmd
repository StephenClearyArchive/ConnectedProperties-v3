@echo off
if not exist Binaries mkdir Binaries
cd Source
..\Util\nuget.exe pack Nito.ConnectedProperties.nuspec -o ..\Binaries
@echo Please rename the .nupkg file to .symbols.nupkg
pause
..\Util\nuget.exe pack Nito.ConnectedProperties.nuspec -o ..\Binaries -Exclude **\*.pdb -Exclude **\*.cs
pause