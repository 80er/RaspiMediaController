cd %~dp0
dotnet publish -r linux-arm
"c:\Program Files (x86)\WinSCP\WinSCP.com" /ini=null /script=DeployToRaspi.cmd