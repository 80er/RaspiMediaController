using System;
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
                // start a MQTT backend e.g. with eclipse-mosquitto and "docker run -it -p 1883:1883 -p 9001:9001 eclipse-mosquitto"  oder toke/mosquitto
                var bus = new MessageBus("RaspiBackend", Environment.GetEnvironmentVariable("MQTT_SERVER"), 9001);
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
