using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices
{
    public class CpuTemperature
    {
        public double Temperature { get; }
        public string Unit => "Celsius";

        public CpuTemperature(double temperature)
        {
            Temperature = temperature;
        }
    }
}
