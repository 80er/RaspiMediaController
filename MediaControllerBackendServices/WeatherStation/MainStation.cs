using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netatmo;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class MainStation : IMainStation
    {
        private Device MainDevice { get; }

        public MainStation()
        {
            MainDevice = Init();
        }

        public string Name => MainDevice.ModuleName;

        public double Noise => MainDevice.DashboardData.Noise;

        public double Pressure => MainDevice.DashboardData.Pressure;

        public IEnumerable<IModule> Modules
        {
            get
            {
                foreach (var module in MainDevice.Modules)
                {
                    yield return ModuleFactory.Create(module);
                }
            }
        }

        public int CO2 => MainDevice.DashboardData.CO2;

        public double Temperature => MainDevice.DashboardData.Temperature;

        public int Humidity => MainDevice.DashboardData.HumidityPercent;

        public ModuleType Type => ModuleType.Main;

        private static Device Init()
        {
            string clientSecret = Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET");
            string clientId = Environment.GetEnvironmentVariable("NETATMO_CLIENT");
            string user = Environment.GetEnvironmentVariable("NETATMO_USER");
            string password = Environment.GetEnvironmentVariable("NETATMO_PASSWORD");
            string deviceId = Environment.GetEnvironmentVariable("NETATMO_DEVICE");
            var clock = NodaTime.SystemClock.Instance;
            var client = new Netatmo.Client(clock, "https://api.netatmo.com/", clientId, clientSecret);
            client.GenerateToken(user, password, new Scope[] { Scope.StationRead }).Wait();

            var token = client.CredentialManager.CredentialToken;
            var station = client.Weather.GetStationsData(deviceId).Result.Body.Devices.First();
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

    }
}
