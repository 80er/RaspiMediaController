using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netatmo;

namespace MediaControllerBackendServices.WeatherStation
{
    internal static class WeatherStationFactory
    {
        public static IMainStation Create(string clientId, string clientSecret, string user, string password, string deviceId)
        {
            var clock = NodaTime.SystemClock.Instance;
            var client = new Netatmo.Client(clock, "https://api.netatmo.com/", clientId, clientSecret);
            client.GenerateToken(user, password, new Scope[] { Scope.StationRead }).Wait();
            
            var token = client.CredentialManager.CredentialToken;
            var station = client.Weather.GetStationsData(deviceId).Result.Body.Devices.First();
            return new MainStation(station);
        }
    }
}
