using System;
using System.Collections.Generic;
using System.Text;
using MQTTnet;
using MQTTnet.Client;

namespace MediaControllerBackendServices.Messaging
{
    internal class MessageBus : IMessageBus
    {
        private IMqttClient MqttClient { get; }
        public MessageBus(string clientId, string uri, int port)
        {
            var mqttFactory = new MqttFactory();
            var options = new MqttClientOptionsBuilder().WithClientId(clientId).WithTcpServer(uri, port).WithCleanSession().Build();

            MqttClient = mqttFactory.CreateMqttClient();

            MqttClient.ConnectAsync(options);
        }
        public void SendMessage(IMessage message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder().WithPayload(message.Payload).WithTopic(message.Topic).Build();
            MqttClient.PublishAsync(mqttMessage);
        }
    }
}
