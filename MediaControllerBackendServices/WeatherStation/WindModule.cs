using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class WindModule : IWindModule
    {
        private Module Module { get; }

        public WindModule(Module module)
        {
            Module = module;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardData>(Module.DashboardData.ToString());
            WindAngle = data.WindAngle;
            WindStrength = data.WindStrength;
        }

        public double WindStrength { get; }

        public double WindAngle { get; }

        public ModuleType Type => ModuleType.Wind;

        public string Name => Module.ModuleName;

        class DashboardData
        {
            public double WindStrength { get; set; }
            public double WindAngle { get; set; }
        }
    }
}
