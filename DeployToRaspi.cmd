option batch on
option confirm off
open pi:raspberry@192.168.1.22 
synchronize remote C:\Users\Chef\Source\Repos\RaspiMediaController\MediaControllerBackendServices\bin\Debug\netcoreapp2.0\linux-arm\publish /home/pi/MediaCenterBackend
synchronize remote C:\Users\Chef\Source\Repos\RaspiMediaController\RaspiMediaControllerFrontend\bin\desktop\ElectronNET.Host-linux-armv7l /home/pi/MediaCenterFrontend
close
exit