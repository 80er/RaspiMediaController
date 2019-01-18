using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class InternalModule : IAirModule
    {
        private Module Module { get; }
        public InternalModule(Module module) => Module = module;
        public ModuleType Type => ModuleType.Internal;
        public string Name => Module.ModuleName;
        public double Temperature => double.Parse((string)Module.DashboardData["Temperature"]);
        public int Humidity => Int32.Parse((string)Module.DashboardData["Humidity"]);
        public int CO2 => Int32.Parse((string)Module.DashboardData["CO2"]);

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine($"Name: {Name} Type: {Type}");
            buffer.AppendLine($"Temperature: {Temperature}°C");
            buffer.AppendLine($"Humidity: {Humidity}%");
            buffer.AppendLine($"CO2: {CO2}");
            return buffer.ToString();
        }
    }
}
