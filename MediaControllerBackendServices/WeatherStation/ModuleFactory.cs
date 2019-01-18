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
            return null;
        }
    }
}
