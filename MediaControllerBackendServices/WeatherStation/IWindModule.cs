using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    interface IWindModule : IModule
    {
        double WindStrength { get; }
        double WindAngle { get; }
        //type = NAModule2
    }
}
