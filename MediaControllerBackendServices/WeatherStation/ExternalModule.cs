using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Energy;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class ExternalModule : ITemperatureModule
    {
        private Module Module { get; }

        public ExternalModule(Module module)
        {
            Module = module;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardData>(Module.DashboardData.ToString());
            Temperature = data.Temperature;
            Humidity = data.Humidity;
        }

        public double Temperature { get; }


        public int Humidity { get; }

        public ModuleType Type => ModuleType.External;

        public string Name => Module.ModuleName;

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine($"Name: {Name} Type: {Type}");
            buffer.AppendLine($"Temperature: {Temperature}°C");
            buffer.AppendLine($"Humidity: {Humidity}%");
            return buffer.ToString();
        }

        class DashboardData
        {
            public double Temperature { get; set; }
            public int Humidity { get; set; }
        }
    }

    
}
