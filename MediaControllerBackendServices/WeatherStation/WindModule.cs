using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class WindModule : IWindModule, IEquatable<IWindModule>
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

        public bool Equals(IWindModule other)
        {
            return Equals(Name, other.Name) && WindStrength.Equals(other.WindStrength) && WindAngle.Equals(other.WindAngle);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WindModule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Module != null ? Module.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ WindStrength.GetHashCode();
                hashCode = (hashCode * 397) ^ WindAngle.GetHashCode();
                return hashCode;
            }
        }
    }
}
