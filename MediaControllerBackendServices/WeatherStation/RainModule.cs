using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class RainModule : IRainModule, IEquatable<IRainModule>
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

        public bool Equals(IRainModule other)
        {
            return Rain.Equals(other.Rain) && Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RainModule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Rain.GetHashCode() * 397) ^ (Module != null ? Module.GetHashCode() : 0);
            }
        }
    }
}
