using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    class RainModule : IRainModule
    {
        public ModuleType Type { get; }
        public string Name { get; }
        public double Rain { get; }
    }
}
