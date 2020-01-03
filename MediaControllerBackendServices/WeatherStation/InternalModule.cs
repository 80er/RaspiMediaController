using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class InternalModule : IAirModule, IEquatable<IAirModule>
    {
        private Module Module { get; }
        private ILog Log { get; set; }
        public InternalModule(Module module, ILog log)
        {
            Log = log;
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

        public bool Equals(IAirModule other)
        {
            return Equals(Name, other.Name) && Temperature.Equals(other.Temperature) && Humidity == other.Humidity && CO2 == other.CO2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InternalModule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Module != null ? Module.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Temperature.GetHashCode();
                hashCode = (hashCode * 397) ^ Humidity;
                hashCode = (hashCode * 397) ^ CO2;
                return hashCode;
            }
        }
    }
}
