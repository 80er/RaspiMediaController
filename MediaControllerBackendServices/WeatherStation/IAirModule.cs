using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface IAirModule : ITemperatureModule
    {
        int CO2 { get; }
        // type NAModule4
    }
}
