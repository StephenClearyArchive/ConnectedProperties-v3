@echo off

cd Help\current

copy /Y ..\..\Source\ConnectedProperties\bin\Release\ConnectedProperties.dll .
copy /Y ..\..\Source\ConnectedProperties\bin\Release\ConnectedProperties.xml .

..\..\Util\FixXmlDocumentation.exe --pre

pause