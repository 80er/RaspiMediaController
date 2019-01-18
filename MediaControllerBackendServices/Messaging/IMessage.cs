using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControllerBackendServices.Messaging
{
    interface IMessage
    {
        string Topic { get; }
        string Payload { get; }
    }
}
