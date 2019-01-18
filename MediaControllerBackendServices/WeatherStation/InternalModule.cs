using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    class InternalModule : IAirModule
    {
        public ModuleType Type { get; }
        public string Name { get; }
        public double Temperature { get; }
        public int Humidity { get; }
        public int CO2 { get; }
    }
}
