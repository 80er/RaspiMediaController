cd %~dp0
cd RaspiMediaControllerFrontend
dotnet publish -r linux-arm
cd ../RaspiMediaControllerFrontend
REM dotnet publish -r linux-arm --output "obj\desktop\linux-arm\bin"
REM cd obj\desktop\linux-arm
REM xcopy /Y ..\linux\api .\api\
REM xcopy /Y ..\linux\main.js .\
REM xcopy /Y ..\linux\package.json .\
REM xcopy /Y ..\linux\package-lock.json .\
dotnet electronize build /target linux /electron-arch armv7l
cd ..
"c:\Program Files (x86)\WinSCP\WinSCP.com" /ini=null /script=DeployToRaspi.cmd