using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using MediaControllerBackendServices.Messaging;
using MediaControllerBackendServices.WeatherStation;
using Newtonsoft.Json;

namespace MediaControllerBackendServices.Broker
{
    internal class WeatherBroker
    {
        private IMainStation MainStation { get; }
        private IMessageBus MessageBus { get; }
        private Timer Timer { get; }
        private static string myTopic = "weather_data";


        public WeatherBroker(IMessageBus messageBus)
        {
            MessageBus = messageBus;
            Timer = new Timer(30000);
            Timer.Elapsed += TimerOnElapsed;
            Timer.Enabled = true;
            TimerOnElapsed(null, null);
            Timer.Start();
        }

        private static object myLockObject = new object();
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (myLockObject)
            {
                // TODO: have to inject somehow main station....
                var station = new MainStation();
                SendMesage(MessageBus, (IAirModule)station);
                foreach (var module in station.GetModules())
                {
                    SendMesage(MessageBus, module);
                }
            }
        }

        private static void SendMesage(IMessageBus bus, IModule module)
        {
            var payload = JsonConvert.SerializeObject(module);
            var message = new Message(myTopic, payload);
            bus.SendMessage(message);
        }

    }
}
