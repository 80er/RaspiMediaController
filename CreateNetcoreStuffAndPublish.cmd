cd %~dp0
cd RaspiMediaControllerFrontend
dotnet publish -r linux-arm
cd ../RaspiMediaControllerFrontend
dotnet electronize build /target linux /electron-arch armv7l
dotnet publish -r linux-arm
robocopy /MIR C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\Debug\netcoreapp2.0\linux-arm\publish C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\desktop\ElectronNET.Host-linux-armv7l\resources\app\bin
REM cd obj\desktop\linux-arm
REM xcopy /Y ..\linux\api .\api\
REM xcopy /Y ..\linux\main.js .\
REM xcopy /Y ..\linux\package.json .\
REM xcopy /Y ..\linux\package-lock.json .\
REM dotnet publish -r linux-arm --output C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\desktop\ElectronNET.Host-linux-armv7l\resources\app\bin

cd ..
"c:\Program Files (x86)\WinSCP\WinSCP.com" /ini=null /script=DeployToRaspi.cmd