using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class RainModule : IRainModule
    {
        public ModuleType Type => ModuleType.Rain;
        public string Name => Module.ModuleName;
        public double Rain { get; }
        private Module Module { get; }

        public RainModule(Module module)
        {
            Module = module;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardData>(Module.DashboardData.ToString());
            Rain = data.Rain;
        }

        class DashboardData
        {
            public double Rain { get; set; }
        }
    }
}
