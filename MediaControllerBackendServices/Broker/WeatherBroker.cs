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
        private bool myResendAll;
        private static Dictionary<string, object> myModuleCache = new Dictionary<string, object>();

        public WeatherBroker(IMessageBus messageBus)
        {
            MessageBus = messageBus;
            Timer = new Timer(30000);
            Timer.Elapsed += TimerOnElapsed;
            Timer.Enabled = true;
            TimerOnElapsed(null, null);
            Timer.Start();
            messageBus.MessageReceived += MessageBusOnMessageReceived;
        }

        private void MessageBusOnMessageReceived(object sender, MessageReceivedArgs e)
        {
            if (e.Message.Topic == myTopic && e.Message.Payload == "resend_all")
            {
                myResendAll = true;
                TimerOnElapsed(null, null);
            }
        }

        private static object myLockObject = new object();
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (myLockObject)
            {
                try
                {
                    // TODO: have to inject somehow main station....
                    var station = new MainStation();
                    SendMesage(MessageBus, (IAirModule)station, myResendAll);
                    foreach (var module in station.GetModules())
                    {
                        SendMesage(MessageBus, module, myResendAll);
                    }

                    myResendAll = false;
                }
                catch (Exception exception)
                {
                   Console.WriteLine("Something went wrong when accessing weather data!");
                   Console.WriteLine(exception);
                }
                
            }
        }

        private static void SendMesage(IMessageBus bus, IModule module, bool sendForSure)
        {
            bool freshlyAdded = false;
            if (!myModuleCache.ContainsKey(module.Name))
            {
                myModuleCache.Add(module.Name, module);
                freshlyAdded = true;
            }

            if (freshlyAdded || !myModuleCache[module.Name].Equals(module) || sendForSure)
            {
                myModuleCache[module.Name] = module;
                var payload = JsonConvert.SerializeObject(module);
                var message = new Message(myTopic, payload);
                bus.SendMessage(message);
            }
        }

    }
}
