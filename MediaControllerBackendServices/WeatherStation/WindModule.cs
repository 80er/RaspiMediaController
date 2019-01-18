using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.WeatherStation
{
    class WindModule : IWindModule
    {
        public double WindStrength => throw new NotImplementedException();

        public double WindAngle => throw new NotImplementedException();

        public ModuleType Type => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();
    }
}
