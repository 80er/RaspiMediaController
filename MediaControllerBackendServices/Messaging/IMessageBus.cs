using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.Messaging
{
    interface IMessageBus
    {
        void SendMessage(IMessage message);
    }
}
