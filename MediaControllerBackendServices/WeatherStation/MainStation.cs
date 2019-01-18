using System;
using System.Collections.Generic;
using System.Text;
using Netatmo.Models.Client.Weather.StationsData;

namespace MediaControllerBackendServices.WeatherStation
{
    class MainStation : IMainStation
    {
        private Device MainDevice { get; }

        public MainStation(Device mainDevice) => MainDevice = mainDevice;

        public string Name => MainDevice.ModuleName;

        public double Noise => MainDevice.DashboardData.Noise;

        public double Pressure => MainDevice.DashboardData.Pressure;

        public IEnumerable<IModule> Modules => throw new NotImplementedException();

        public int CO2 => MainDevice.DashboardData.CO2;

        public double Temperature => MainDevice.DashboardData.Temperature;

        public int Humidity => MainDevice.DashboardData.HumidityPercent;

        public ModuleType Type => ModuleType.Main;

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
