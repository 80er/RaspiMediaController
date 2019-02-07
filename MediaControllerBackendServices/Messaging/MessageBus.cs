using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace MediaControllerBackendServices.Messaging
{
    internal class MessageBus : IMessageBus
    {
        private IMqttClient MqttClient { get; set; }
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
                await MqttClient.ConnectAsync(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private IMqttClient GetOrCreateClient()
        {
            if (MqttClient != null)
            {
                return MqttClient;
            }
            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            mqttClient.Connected += MqttClientOnConnected;
            mqttClient.ApplicationMessageReceived += MqttClientOnApplicationMessageReceived;
            mqttClient.Disconnected += MqttClientOnDisconnected;
            return mqttClient;
        }

        private void MqttClientOnDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            var client = GetOrCreateClient();
            while (!client.IsConnected)
            {
                Thread.Sleep(500);
                Connect();
            }
        }

        private static IMqttClientOptions CreateOptions()
        {
            return new MqttClientOptionsBuilder().WithClientId(ClientId).WithWebSocketServer($"{Uri}:{Port}").WithCleanSession(true).WithKeepAlivePeriod(TimeSpan.FromSeconds(10)).Build();
        }

        private void MqttClientOnApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
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

        private async void MqttClientOnConnected(object sender, MqttClientConnectedEventArgs e)
        {
            MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("#").WithExactlyOnceQoS().Build());
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
