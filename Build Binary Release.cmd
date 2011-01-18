@echo off

if not exist Binaries mkdir Binaries
if not exist Binaries\ConnectedProperties mkdir Binaries\ConnectedProperties

if not exist Binaries\ConnectedProperties\Net40 mkdir Binaries\ConnectedProperties\Net40
if not exist Binaries\ConnectedProperties\Net40\Debug mkdir Binaries\ConnectedProperties\Net40\Debug
if not exist Binaries\ConnectedProperties\Net40\CodeContracts mkdir Binaries\ConnectedProperties\Net40\CodeContracts

if not exist Binaries\ConnectedProperties\SL4 mkdir Binaries\ConnectedProperties\SL4
if not exist Binaries\ConnectedProperties\SL4\Debug mkdir Binaries\ConnectedProperties\SL4\Debug
if not exist Binaries\ConnectedProperties\SL4\CodeContracts mkdir Binaries\ConnectedProperties\SL4\CodeContracts

copy /Y Source\ConnectedProperties\bin\Debug\ConnectedProperties.dll Binaries\ConnectedProperties\Net40\Debug
copy /Y Source\ConnectedProperties\bin\Debug\ConnectedProperties.pdb Binaries\ConnectedProperties\Net40\Debug

copy /Y Source\ConnectedProperties\bin\Release\ConnectedProperties.dll Binaries\ConnectedProperties\Net40
copy /Y Source\ConnectedProperties\bin\Release\ConnectedProperties.pdb Binaries\ConnectedProperties\Net40
copy /Y Source\ConnectedProperties\bin\Release\ConnectedProperties.xml Binaries\ConnectedProperties\Net40
copy /Y Source\ConnectedProperties\bin\Release\CodeContracts\ConnectedProperties.Contracts.dll Binaries\ConnectedProperties\Net40\CodeContracts
copy /Y Source\ConnectedProperties\bin\Release\CodeContracts\ConnectedProperties.Contracts.pdb Binaries\ConnectedProperties\Net40\CodeContracts

copy /Y "Source\ConnectedProperties (SL)\bin\Debug\ConnectedProperties.dll" Binaries\ConnectedProperties\SL4\Debug
copy /Y "Source\ConnectedProperties (SL)\bin\Debug\ConnectedProperties.pdb" Binaries\ConnectedProperties\SL4\Debug

copy /Y "Source\ConnectedProperties (SL)\bin\Release\ConnectedProperties.dll" Binaries\ConnectedProperties\SL4
copy /Y "Source\ConnectedProperties (SL)\bin\Release\ConnectedProperties.pdb" Binaries\ConnectedProperties\SL4
copy /Y "Source\ConnectedProperties (SL)\bin\Release\ConnectedProperties.xml" Binaries\ConnectedProperties\SL4
copy /Y "Source\ConnectedProperties (SL)\bin\Release\CodeContracts\ConnectedProperties.Contracts.dll" Binaries\ConnectedProperties\SL4\CodeContracts
copy /Y "Source\ConnectedProperties (SL)\bin\Release\CodeContracts\ConnectedProperties.Contracts.pdb" Binaries\ConnectedProperties\SL4\CodeContracts

pause