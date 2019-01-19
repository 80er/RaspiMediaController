using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.Messaging
{
    class Message : IMessage
    {
        public Message(string topic, string payload)
        {
            Topic = topic;
            Payload = payload;
        }
        public string Topic { get; }

        public string Payload { get; }
    }
}
