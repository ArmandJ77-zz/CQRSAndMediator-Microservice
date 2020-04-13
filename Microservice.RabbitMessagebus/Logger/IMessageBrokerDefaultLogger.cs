using Microservice.RabbitMessageBroker.Models;
using System;

namespace Microservice.RabbitMessageBroker.Logger
{
    public interface IMessageBrokerDefaultLogger
    {
        void MessageReceived<T>(T eventModel, MessageReceivedLogContext context, long swElapsedMilliseconds);
        void ProcessingSucceeded<T>(T eventModel, MessageReceivedLogContext context, long swElapsedMilliseconds);
        void ProcessingFailed<T>(T eventModel, MessageReceivedLogContext context, Exception exception, long swElapsedMilliseconds);
        void PublishedSucceeded(string topic, string bodyAsString);

        void UnsubFromTopic(string topic, string subscriptionId);

        void SubedAndCompeting(string topic, string subscriptionId);
        void ConnectionLostReconnecting(string topic, string subscriptionId);

        void UnableToConnect(string topic,
            string subscriptionId,
            int attempt,
            int retryDelay,
            string host,
            int port);
    }
}
