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
                Console.WriteLine("Return ExternalModule");
                return new ExternalModule(module);
            }

            if (module.Type == "NAModule4")
            {
                Console.WriteLine("Return InternalModule");
                return new InternalModule(module);
            }

            if (module.Type == "NAModule2")
            {
                Console.WriteLine("Return WindModule");
                return new WindModule(module);
            }

            if (module.Type == "NAModule3")
            {
                Console.WriteLine("Return RainModule");
                return new RainModule(module);
            }
            Console.WriteLine("Returning null");
            return null;
        }
    }
}
