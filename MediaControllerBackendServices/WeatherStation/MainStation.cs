using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netatmo;
using Netatmo.Models.Client.Air;
using Netatmo.Models.Client.Air.HomesCoachs;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class MainStation : IMainStation, IEquatable<IMainStation>
    {
        private Device MainDevice { get; set; }
        private string _deviceID;
        private Client _client;
        public MainStation()
        {
            Init();
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

        public void Refresh()
        {
            try
            {
                MainDevice = _client.Weather.GetStationsData(_deviceID).Result.Body.Devices.First();
            } catch (Exception ex)
            {
                Console.WriteLine($"Exception in refreshing station. {ex.Message}{Environment.NewLine}{ex}");
                _client.RefreshToken();
            }
            
        }

        private void Init()
        {
            string clientSecret = Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET").Trim();
            string clientId = Environment.GetEnvironmentVariable("NETATMO_CLIENT").Trim();
            _deviceID = Environment.GetEnvironmentVariable("NETATMO_DEVICE").Trim();
            string token = Environment.GetEnvironmentVariable("NETATMO_TOKEN").Trim();
            string refreshToken = Environment.GetEnvironmentVariable("NETATMO_REFRESH_TOKEN").Trim();
            var clock = NodaTime.SystemClock.Instance;
            _client = new Netatmo.Client(clock, "https://api.netatmo.com/", clientId, clientSecret);
            Device station = null;
            try
            {
                _client.CredentialManager.ProvideOAuth2Token(token, refreshToken);
                MainDevice = _client.Weather.GetStationsData(_deviceID).Result.Body.Devices.First();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in accessing weather data: {e.Message}.{Environment.NewLine}{e}");
            }
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
