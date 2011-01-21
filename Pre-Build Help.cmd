@echo off

cd Help\current

copy /Y ..\..\Source\ConnectedProperties\bin\Release\ConnectedProperties.dll .
copy /Y ..\..\Source\ConnectedProperties\bin\Release\ConnectedProperties.xml .
copy /Y ..\Topics\*.aml .

..\..\Util\FixXmlDocumentation.exe --pre

pause