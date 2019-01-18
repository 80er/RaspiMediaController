using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class ExternalModule : ITemperatureModule
    {
        private Module Module { get; }
        public ExternalModule(Module module) => Module = module;
        public double Temperature => double.Parse((string)Module.DashboardData["Temperature"]);

        public int Humidity => Int32.Parse((string)Module.DashboardData["Humidity"]);

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
    }
}
