using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    static class ModuleFactory
    {
        public static IModule Create(Module module)
        {
            if (module.Type == "NAModule1")
            {
                return new ExternalModule(module);
            }

            if (module.Type == "NAModule4")
            {
                return new InternalModule(module);
            }

            if (module.Type == "NAModule2")
            {
                return new WindModule(module);
            }

            if (module.Type == "NAModule3")
            {
                return new RainModule(module);
            }
            return null;
        }
    }
}
