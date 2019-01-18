using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface IModule
    {
        ModuleType Type { get; }
        string Name { get; }   
    }
}
