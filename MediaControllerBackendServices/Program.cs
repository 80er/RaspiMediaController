using System;
using System.IO;
using System.Reflection;
using System.Threading;
using MediaControllerBackendServices.Broker;
using MediaControllerBackendServices.Messaging;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
               Console.WriteLine("Starting up");
               Console.WriteLine($"NETATMO_CLIENT_SECRET={Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET")}");
               Console.WriteLine($"NETATMO_CLIENT={Environment.GetEnvironmentVariable("NETATMO_CLIENT")}");
               Console.WriteLine($"NETATMO_USER={Environment.GetEnvironmentVariable("NETATMO_USER")}");
               Console.WriteLine($"NETATMO_PASSWORD={Environment.GetEnvironmentVariable("NETATMO_PASSWORD")}");
               Console.WriteLine($"NETATMO_DEVICE={Environment.GetEnvironmentVariable("NETATMO_DEVICE")}");
               Console.WriteLine($"MQTT_SERVER={Environment.GetEnvironmentVariable("MQTT_SERVER")}");
                var mqtt = Environment.GetEnvironmentVariable("MQTT_SERVER") ?? "mosquitto";
               Console.WriteLine($"Will use {mqtt} as mqtt broker");
                var bus = new MessageBus("RaspiBackend", mqtt, 9001);
                var broker = new WeatherBroker(bus);
                var timer = new TimeBroker(bus);
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
                Environment.Exit(1);
            }
            
        }
    }
}
