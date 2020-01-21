using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Energy;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class ExternalModule : ITemperatureModule, IEquatable<ITemperatureModule>
    {
        private Module Module { get; }

        public ExternalModule(Module module)
        {
            Module = module;
            Console.WriteLine($"{Module.DashboardData}");
            if (Module.DashboardData != null)
            {
                var data =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardData>(Module.DashboardData.ToString());
                Temperature = data.Temperature;
                Humidity = data.Humidity;
            }
            else
            {
                Console.WriteLine("Module.DashboardData was null!");
            }
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

        public bool Equals(ITemperatureModule other)
        {
            return Equals(Name, other.Name) && Temperature.Equals(other.Temperature) && Humidity == other.Humidity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExternalModule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Module != null ? Module.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Temperature.GetHashCode();
                hashCode = (hashCode * 397) ^ Humidity;
                return hashCode;
            }
        }
    }

    
}
