@echo off

if not exist Binaries mkdir Binaries
if not exist Binaries\ConnectedProperties mkdir Binaries\ConnectedProperties

if not exist Binaries\ConnectedProperties\Net40 mkdir Binaries\ConnectedProperties\Net40
if not exist Binaries\ConnectedProperties\Net40\Debug mkdir Binaries\ConnectedProperties\Net40\Debug
if not exist Binaries\ConnectedProperties\Net40\Release mkdir Binaries\ConnectedProperties\Net40\Release
if not exist Binaries\ConnectedProperties\Net40\Release\CodeContracts mkdir Binaries\ConnectedProperties\Net40\Release\CodeContracts

if not exist Binaries\ConnectedProperties\SL4 mkdir Binaries\ConnectedProperties\SL4
if not exist Binaries\ConnectedProperties\SL4\Debug mkdir Binaries\ConnectedProperties\SL4\Debug
if not exist Binaries\ConnectedProperties\SL4\Release mkdir Binaries\ConnectedProperties\SL4\Release
if not exist Binaries\ConnectedProperties\SL4\Release\CodeContracts mkdir Binaries\ConnectedProperties\SL4\Release\CodeContracts

copy /Y Help\current\Help\ConnectedProperties.chm Binaries\ConnectedProperties

copy /Y Source\ConnectedProperties\bin\Debug\ConnectedProperties.dll Binaries\ConnectedProperties\Net40\Debug
copy /Y Source\ConnectedProperties\bin\Debug\ConnectedProperties.pdb Binaries\ConnectedProperties\Net40\Debug
copy /Y Help\current\Help\ConnectedProperties.xml Binaries\ConnectedProperties\Net40\Debug

copy /Y Source\ConnectedProperties\bin\Release\ConnectedProperties.dll Binaries\ConnectedProperties\Net40\Release
copy /Y Source\ConnectedProperties\bin\Release\ConnectedProperties.pdb Binaries\ConnectedProperties\Net40\Release
copy /Y Help\current\Help\ConnectedProperties.xml Binaries\ConnectedProperties\Net40\Release
copy /Y Source\ConnectedProperties\bin\Release\CodeContracts\ConnectedProperties.Contracts.dll Binaries\ConnectedProperties\Net40\Release\CodeContracts
copy /Y Source\ConnectedProperties\bin\Release\CodeContracts\ConnectedProperties.Contracts.pdb Binaries\ConnectedProperties\Net40\Release\CodeContracts

copy /Y "Source\ConnectedProperties (SL)\bin\Debug\ConnectedProperties.dll" Binaries\ConnectedProperties\SL4\Debug
copy /Y "Source\ConnectedProperties (SL)\bin\Debug\ConnectedProperties.pdb" Binaries\ConnectedProperties\SL4\Debug
copy /Y Help\current\Help\ConnectedProperties.xml Binaries\ConnectedProperties\SL4\Debug

copy /Y "Source\ConnectedProperties (SL)\bin\Release\ConnectedProperties.dll" Binaries\ConnectedProperties\SL4\Release
copy /Y "Source\ConnectedProperties (SL)\bin\Release\ConnectedProperties.pdb" Binaries\ConnectedProperties\SL4\Release
copy /Y Help\current\Help\ConnectedProperties.xml Binaries\ConnectedProperties\SL4\Release
copy /Y "Source\ConnectedProperties (SL)\bin\Release\CodeContracts\ConnectedProperties.Contracts.dll" Binaries\ConnectedProperties\SL4\Release\CodeContracts
copy /Y "Source\ConnectedProperties (SL)\bin\Release\CodeContracts\ConnectedProperties.Contracts.pdb" Binaries\ConnectedProperties\SL4\Release\CodeContracts

pause