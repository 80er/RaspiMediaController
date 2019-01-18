﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface ITemperatureModule : IModule
    {
        double Temperature { get; }
        int Humidity { get; }
        // type NAModule1
    }
}
