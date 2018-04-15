using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediaControllerBackendServices
{
    class LinuxCpuTemperatureReader : ICpuTemperatureReader
    {
        private String PathToTemperatureFile = "/sys/class/thermal/thermal_zone0/temp";

        public CpuTemperature GetCurrentTemperature()
        {
            var content = File.ReadAllText(PathToTemperatureFile);
            var temp = Int32.Parse(content);
            var tempInDouble = (double)temp / 1000.0;
            return new CpuTemperature(tempInDouble);
        }
    }
}
