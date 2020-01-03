using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    static class ModuleFactory
    {
        private static ILog _log = LogManager.GetLogger(typeof(ModuleFactory));
        public static IModule Create(Module module)
        {
            if (module.Type == "NAModule1")
            {
                _log.Info("Return ExternalModule");
                return new ExternalModule(module, LogManager.GetLogger(typeof(ExternalModule)));
            }

            if (module.Type == "NAModule4")
            {
                _log.Info("Return InternalModule");
                return new InternalModule(module, LogManager.GetLogger(typeof(InternalModule)));
            }

            if (module.Type == "NAModule2")
            {
                _log.Info("Return WindModule");
                return new WindModule(module, LogManager.GetLogger(typeof(WindModule)));
            }

            if (module.Type == "NAModule3")
            {
                _log.Info("Return RainModule");
                return new RainModule(module, LogManager.GetLogger(typeof(RainModule)));
            }
            _log.Warn("Returning null");
            return null;
        }
    }
}
