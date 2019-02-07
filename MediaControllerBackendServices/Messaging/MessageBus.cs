using System;
using System.Collections.Generic;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace MediaControllerBackendServices.Messaging
{
    internal class MessageBus : IMessageBus
    {
        private IMqttClient MqttClient { get; }
        public MessageBus(string clientId, string uri, int port)
        {
            var mqttFactory = new MqttFactory();
            var options = new MqttClientOptionsBuilder().WithClientId(clientId).WithWebSocketServer($"{uri}:{port}").WithCleanSession(true).WithKeepAlivePeriod(TimeSpan.FromSeconds(10)).Build();

            MqttClient = mqttFactory.CreateMqttClient();
            

            MqttClient.Connected += MqttClientOnConnected;

            MqttClient.ConnectAsync(options);

            MqttClient.ApplicationMessageReceived += MqttClientOnApplicationMessageReceived;

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
