using System;
using MediaControllerBackendServices.WeatherStation;
using MQTTnet;
using MQTTnet.Client;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientSecret = Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET");
            string client = Environment.GetEnvironmentVariable("NETATMO_CLIENT");
            string user = Environment.GetEnvironmentVariable("NETATMO_USER");
            string password = Environment.GetEnvironmentVariable("NETATMO_PASSWORD");
            string device = Environment.GetEnvironmentVariable("NETATMO_DEVICE");
            var mainstation = WeatherStationFactory.Create(client, clientSecret, user, password, device);
            Console.WriteLine(mainstation.ToString());
            foreach (var module in mainstation.Modules)
            {
                Console.WriteLine(module);
            }
            CreateMqtt();
        }

        private static async void CreateMqtt()
        {
            try
            {
                var mqttFactory = new MqttFactory();
                var options = new MqttClientOptionsBuilder().WithClientId("MediaServerBackend").WithTcpServer("127.0.0.1", 1883).WithCleanSession().Build();

                var client = mqttFactory.CreateMqttClient();

                var result = client.ConnectAsync(options).Result;
                var message = new MqttApplicationMessageBuilder().WithPayload("juhu").WithTopic("test_topic").Build();
                await client.PublishAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}
