cd %~dp0
cd MediaControllerBackendServices
dotnet publish -r linux-arm
cd ../RaspiMediaControllerFrontend
dotnet electronize build /target custom linux-arm;linux /electron-arch armv7l
REM dotnet publish -r linux-arm
REM robocopy /MIR C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\Debug\netcoreapp2.1\linux-arm\publish C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\desktop\ElectronNET.Host-linux-armv7l\resources\app\bin

cd ..
"c:\Program Files (x86)\WinSCP\WinSCP.com" /ini=null /script=DeployToRaspi.cmd