using NServiceBus;
using System;
namespace Messages
{
    public interface IMailMessageDeliveryRequest : ICommand
    {
        string MessageId { get; set; }
    }
}
