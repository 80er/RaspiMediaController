using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MediaControllerBackendServices.Messaging
{
    internal class MessageBus : IMessageBus
    {
        private IManagedMqttClient MqttClient { get; set; }
        private static string ClientId { get; set; }
        private static string Uri { get; set; }
        private static int Port { get; set; }
        public MessageBus(string clientId, string uri, int port)
        {
           ClientId = clientId;
           Uri = uri;
           Port = port;
           Connect();
        }

        private async void Connect()
        {
           Console.WriteLine("Connecting");
            MqttClient = GetOrCreateClient();
            var options = CreateOptions();
            try
            {
                MqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
                await MqttClient.SubscribeAsync(new Collection<MqttTopicFilter>{new MqttTopicFilterBuilder().WithTopic("weather_data").Build(), new MqttTopicFilterBuilder().WithTopic("time_data").Build() });
               Console.WriteLine("Subscribed");
                await MqttClient.StartAsync(options);
               Console.WriteLine("Started");
               //MqttClient.ApplicationMessageProcessedAsync += MqttClientOnApplicationMessageProcessedAsync;

                MqttClient.DisconnectedAsync += MqttClientOnDisconnectedAsync;
                MqttClient.ConnectedAsync += MqttClientOnConnectedAsync; 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private Task MqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            Console.WriteLine($"Received message for topic {arg.ApplicationMessage.Topic}");
            var messageReceived = MessageReceived;
            if (messageReceived != null)
            {
                Console.WriteLine("Will raise event");
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                var message = enc.GetString(arg.ApplicationMessage.Payload);
                Console.WriteLine($"Event raised: {arg.ApplicationMessage.Topic}, {message},{arg.ApplicationMessage.Retain},{arg.ApplicationMessage.QualityOfServiceLevel}");
                messageReceived(this, new MessageReceivedArgs(new Message(arg.ApplicationMessage.Topic, message)));
            }

            return Task.CompletedTask;
        }

        private Task MqttClientOnDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Bus not connected");
                if (arg.Exception != null)
                {
                    Console.WriteLine(arg.Exception);
                }
            });
        }

        private Task MqttClientOnConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            return Task.Run(() => Console.WriteLine("Mqtt connected"));
        }

        //private Task MqttClientOnApplicationMessageProcessedAsync(ApplicationMessageProcessedEventArgs arg)
        //{
        //    Console.WriteLine($"Received message for topic {arg.ApplicationMessage.ApplicationMessage.Topic}");
        //    var messageReceived = MessageReceived;
        //    if (messageReceived != null)
        //    {
        //        Console.WriteLine("Will raise event");
        //        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //        var message = enc.GetString(arg.ApplicationMessage.ApplicationMessage.Payload);
        //        Console.WriteLine($"Event raised: {arg.ApplicationMessage.ApplicationMessage.Topic}, {message},{arg.ApplicationMessage.ApplicationMessage.Retain},{arg.ApplicationMessage.ApplicationMessage.QualityOfServiceLevel}");
        //        messageReceived(this, new MessageReceivedArgs(new Message(arg.ApplicationMessage.ApplicationMessage.Topic, message)));
        //    }

        //    return Task.CompletedTask;
        //}

        private IManagedMqttClient GetOrCreateClient()
        {
            if (MqttClient != null)
            {
               Console.WriteLine("Client existed");
                return MqttClient;
            }
           Console.WriteLine("Creating new client");
            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            MqttClient = mqttClient;
            return MqttClient;
        }

        private static ManagedMqttClientOptions CreateOptions()
        { 
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId(ClientId)
                .WithTcpServer($"{Uri}")
                .WithCleanSession(true)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .Build()).Build();
            return options;
        }

        private void MqttClientOnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
          
        }


        public event EventHandler<MessageReceivedArgs> MessageReceived;

        public void SendMessage(IMessage message)
        {
           Console.WriteLine("Will send message");
            if (!MqttClient.IsConnected)
            {
               Console.WriteLine("Aborted because not connected");
                return;
            }
            var mqttMessage = new MqttApplicationMessageBuilder().
                WithPayload(message.Payload).
                WithTopic(message.Topic).
                WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).
                WithRetainFlag(false).
                Build();
            MqttClient.InternalClient.PublishAsync(mqttMessage, CancellationToken.None);
        }
    }
}
