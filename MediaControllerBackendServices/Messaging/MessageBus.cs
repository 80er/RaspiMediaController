using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
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
                await MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("#").Build());
               Console.WriteLine("Subscribed");
                await MqttClient.StartAsync(options);
               Console.WriteLine("Started");
                MqttClient.UseApplicationMessageReceivedHandler(MqttClientOnApplicationMessageReceived);
                MqttClient.UseDisconnectedHandler(MqttDisconnected);
                MqttClient.UseConnectedHandler(MqttConnected);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private Task MqttConnected(MqttClientConnectedEventArgs arg)
        {
            return Task.Run(() =>Console.WriteLine("Mqtt connected"));
        }

        private Task MqttDisconnected(MqttClientDisconnectedEventArgs arg)
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
                .WithWebSocketServer($"{Uri}:{Port}")
                .WithCleanSession(true)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .Build()).Build();
            return options;
        }

        private void MqttClientOnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
           Console.WriteLine($"Received message for topic {e.ApplicationMessage.Topic}");
            var messageReceived = MessageReceived;
            if (messageReceived != null)
            {
               Console.WriteLine("Will raise event");
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                var message = enc.GetString(e.ApplicationMessage.Payload);
               Console.WriteLine($"Event raised: {e.ApplicationMessage.Topic}, {message},{e.ApplicationMessage.Retain},{e.ApplicationMessage.QualityOfServiceLevel}");
                messageReceived(this, new MessageReceivedArgs(new Message(e.ApplicationMessage.Topic,message )));
            }
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
                WithExactlyOnceQoS().
                WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).
                WithRetainFlag(false).
                Build();
            MqttClient.PublishAsync(mqttMessage);
        }
    }
}
