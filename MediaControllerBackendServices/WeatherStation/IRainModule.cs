using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface IRainModule : IModule
    {
        double Rain { get; }
        //type = NAModule3
    }
}
