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
            MqttClient = GetOrCreateClient();
            var options = CreateOptions();
            try
            {
                await MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("#").Build());
                await MqttClient.StartAsync(options);
                MqttClient.UseApplicationMessageReceivedHandler(MqttClientOnApplicationMessageReceived);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private IManagedMqttClient GetOrCreateClient()
        {
            if (MqttClient != null)
            {
                return MqttClient;
            }
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
            var messageReceived = MessageReceived;
            if (messageReceived != null)
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                var message = enc.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Message received: {e.ApplicationMessage.Topic}, {message},{e.ApplicationMessage.Retain},{e.ApplicationMessage.QualityOfServiceLevel}");
                
                 
                messageReceived(this, new MessageReceivedArgs(new Message(e.ApplicationMessage.Topic,message )));
            }
        }


        public event EventHandler<MessageReceivedArgs> MessageReceived;

        public void SendMessage(IMessage message)
        {
            if (!MqttClient.IsConnected)
            {
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
