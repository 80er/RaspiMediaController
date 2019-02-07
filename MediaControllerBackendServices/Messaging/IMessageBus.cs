using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.Messaging
{
    interface IMessageBus
    {
        event EventHandler<MessageReceivedArgs> MessageReceived;
        void SendMessage(IMessage message);
    }
}
