using System;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using MediaControllerBackendServices.Broker;
using MediaControllerBackendServices.Messaging;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var log = LogManager.GetLogger(typeof(Program));
            try
            {
                log.Info("Starting up");
                log.Info($"NETATMO_CLIENT_SECRET={Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET")}");
                log.Info($"NETATMO_CLIENT={Environment.GetEnvironmentVariable("NETATMO_CLIENT")}");
                log.Info($"NETATMO_USER={Environment.GetEnvironmentVariable("NETATMO_USER")}");
                log.Info($"NETATMO_PASSWORD={Environment.GetEnvironmentVariable("NETATMO_PASSWORD")}");
                log.Info($"NETATMO_DEVICE={Environment.GetEnvironmentVariable("NETATMO_DEVICE")}");
                log.Info($"MQTT_SERVER={Environment.GetEnvironmentVariable("MQTT_SERVER")}");
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
                log.Error(e);
                Environment.Exit(1);
            }
            
        }
    }
}
