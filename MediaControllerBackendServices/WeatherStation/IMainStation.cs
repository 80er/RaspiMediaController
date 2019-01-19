using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface IMainStation : IAirModule
    {
        // type = NAMain
        double Noise { get; }
        double Pressure { get; }
        IEnumerable<IModule> GetModules();
    }
}
