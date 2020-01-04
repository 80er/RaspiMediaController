using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Netatmo;
using Netatmo.Models.Client.Air;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class MainStation : IMainStation, IEquatable<IMainStation>
    {
        static ILog _log = LogManager.GetLogger(typeof(MainStation));
        private Device MainDevice { get; }

        public MainStation()
        {
            MainDevice = Init();
        }

        public string Name {
            get
            {
                if(MainDevice != null)
                    return MainDevice.ModuleName;
                return "Not Connected";
            }
        }

        public double Noise 
        {
            get
            {
                if(MainDevice != null) return MainDevice.DashboardData.Noise;
                return -1.0;
            }
        }

        public double Pressure
        {
            get
            {
                if (MainDevice != null) return MainDevice.DashboardData.Pressure;
                return -1.0;
            }
        }

        public IEnumerable<IModule> GetModules()
        {
            foreach (var module in MainDevice.Modules)
            {
                yield return ModuleFactory.Create(module);
            }
        }

        public int CO2
        {
            get
            {
                if (MainDevice != null) return MainDevice.DashboardData.CO2;
                return -1;
            }
        }

        public double Temperature
        {
            get
            {
                if (MainDevice != null) return MainDevice.DashboardData.Temperature;
                return -1.0;
            }
        }

        public int Humidity
        {
            get
            {
                if (MainDevice != null) return MainDevice.DashboardData.HumidityPercent;
                return -1;
            }
        }

        public ModuleType Type => ModuleType.Main;

        private static Device Init()
        {
            string clientSecret = Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET").Trim();
            string clientId = Environment.GetEnvironmentVariable("NETATMO_CLIENT").Trim();
            string user = Environment.GetEnvironmentVariable("NETATMO_USER").Trim();
            string password = Environment.GetEnvironmentVariable("NETATMO_PASSWORD").Trim();
            string deviceId = Environment.GetEnvironmentVariable("NETATMO_DEVICE").Trim();
            var clock = NodaTime.SystemClock.Instance;
            var client = new Netatmo.Client(clock, "https://api.netatmo.com/", clientId, clientSecret);
            Device station = null;
            try
            { 
                _log.Info($"Will generate token with user: {user} and password {password}");
                client.GenerateToken(user, password, new Scope[] { Scope.StationRead }).Wait();

                var token = client.CredentialManager.CredentialToken;
                _log.Info($"Aquired token: {token.AccessToken}");
                
                client.CredentialManager.RefreshToken().Wait();
                station = client.Weather.GetStationsData(deviceId).Result.Body.Devices.First();
            }
            catch (Exception e)
            {
                _log.Error($"Exception in accessing weather data: {e.Message}", e);
            }
            
            return station;
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.AppendLine($"Name: {Name} and Type:{Type}");
            buffer.AppendLine($"Temperature: {Temperature}°C");
            buffer.AppendLine($"Humidity: {Humidity}%");
            buffer.AppendLine($"CO2: {CO2}");
            buffer.AppendLine($"Pressure: {Pressure}mbar");
            buffer.AppendLine($"Noise: {Noise}db");
            return buffer.ToString();
        }

        public bool Equals(IMainStation other)
        {
            return Name == other.Name && 
                   Temperature == other.Temperature && 
                   CO2 == other.CO2 && 
                   Humidity == other.Humidity &&
                   Pressure == other.Pressure && 
                   Noise == other.Noise;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MainStation) obj);
        }

        public override int GetHashCode()
        {
            return (MainDevice != null ? MainDevice.GetHashCode() : 0);
        }
    }
}
