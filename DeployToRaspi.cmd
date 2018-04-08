option batch on
option confirm off
open pi:raspberry@192.168.1.22
kill-all CpuTempService
cd ~/CPUTempService 
synchronize remote C:\Users\Chef\Source\Repos\RaspiMediaController\CpuTempService\bin\Debug\netcoreapp2.0\linux-arm\publish /home/pi/CPUTempService
sudo ./CpuTempService
close
exit