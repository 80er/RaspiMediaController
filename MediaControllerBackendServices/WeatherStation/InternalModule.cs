using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class InternalModule : IAirModule
    {
        private Module Module { get; }
        public InternalModule(Module module)
        {
            Module = module;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardData>(Module.DashboardData.ToString());
            Temperature = data.Temperature;
            Humidity = data.Humidity;
            CO2 = data.CO2;
        }
        public ModuleType Type => ModuleType.Internal;
        public string Name => Module.ModuleName;
        public double Temperature { get; }
        public int Humidity { get; }
        public int CO2 { get; }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine($"Name: {Name} Type: {Type}");
            buffer.AppendLine($"Temperature: {Temperature}°C");
            buffer.AppendLine($"Humidity: {Humidity}%");
            buffer.AppendLine($"CO2: {CO2}");
            return buffer.ToString();
        }

        class DashboardData
        {
            public double Temperature { get; set; }
            public int Humidity { get; set; }
            public int CO2 { get; set; }
        }
    }
}
