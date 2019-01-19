using System;
using System.Threading;
using MediaControllerBackendServices.Broker;
using MediaControllerBackendServices.Messaging;
using MediaControllerBackendServices.WeatherStation;
using MQTTnet;
using MQTTnet.Client;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            // start a MQTT backend e.g. with eclipse-mosquitto and "docker run -it -p 1883:1883 -p 9001:9001 eclipse-mosquitto"
            var bus = new MessageBus("RaspiBackend", "127.0.0.1", 1883);
            var broker = new WeatherBroker(bus);
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
