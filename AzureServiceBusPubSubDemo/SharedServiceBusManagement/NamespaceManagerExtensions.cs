using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Microsoft.ServiceBus
{
    public static class NamespaceManagerExtensions
    {
        public static NamespaceManager CreateQueueIfNotExists( this NamespaceManager namespaceManager, String queueName )
        {
            if( !namespaceManager.QueueExists( queueName ) )
            {
                try
                {
                    namespaceManager.CreateQueue( queueName );
                }
                catch( MessagingEntityAlreadyExistsException )
                {
                    //NOP
                }
            }

            return namespaceManager;
        }

        public static NamespaceManager CreateTopicIfNotExists( this NamespaceManager namespaceManager, String topicName )
        {
            if( !namespaceManager.TopicExists( topicName ) )
            {
                try
                {
                    namespaceManager.CreateTopic( topicName );
                }
                catch( MessagingEntityAlreadyExistsException )
                {
                    //NOP
                }
            }

            return namespaceManager;
        }

        public static NamespaceManager CreateSubscriptionIfNotExists( this NamespaceManager namespaceManager, String topicName, String subscriptionName )
        {
            if( !namespaceManager.SubscriptionExists( topicName, subscriptionName ) )
            {
                try
                {
                    namespaceManager.CreateSubscription( topicName, subscriptionName );
                }
                catch( MessagingEntityAlreadyExistsException )
                {
                    //NOP
                }
            }

            return namespaceManager;
        }
    }
}