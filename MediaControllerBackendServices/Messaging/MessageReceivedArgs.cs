using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.Messaging
{
    internal class MessageReceivedArgs : EventArgs
    {
        public IMessage Message
        {
            get; 
        }

        public MessageReceivedArgs(IMessage message)
        {
            Message = message;
        }
    }
}
