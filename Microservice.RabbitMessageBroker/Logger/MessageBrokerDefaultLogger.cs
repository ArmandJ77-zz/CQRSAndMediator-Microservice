using Microservice.RabbitMessageBroker.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Microservice.RabbitMessageBroker.Logger
{
    public class MessageBrokerDefaultLogger : IMessageBrokerDefaultLogger
    {
        private readonly ILogger<RabbitMessageBrokerClient> _logger;

        public MessageBrokerDefaultLogger(ILogger<RabbitMessageBrokerClient> logger)
        {
            _logger = logger;
        }

        public void MessageReceived<T>(T eventModel, MessageReceivedLogContext context, long elapsedMilliseconds)
        {
            _logger.LogInformation(
                "RabbitMQ: Message received for topic in worker pool, {@details}", new
                {
                    RabbitMessageBrokerClientMessageReceived = new
                    {
                        topic = context.Topic,
                        subscriptionId = context.SubscriptionId,
                        eventModel,
                        correlationId = context.CorrelationId,
                        elapsedMilliseconds
                    }
                }
            );
        }

        public void ProcessingSucceeded<T>(T eventModel, MessageReceivedLogContext context, long elapsedMilliseconds)
        {
            _logger.LogInformation(
                "RabbitMQ: Message processing succeeded, {@details}", new
                {
                    RabbitMessageBrokerClientMessageProcessed = new
                    {
                        topic = context.Topic,
                        subscriptionId = context.SubscriptionId,
                        correlationId = context.CorrelationId,
                        elapsedMilliseconds
                    }
                }
            );
        }

        public void ProcessingFailed<T>(T eventModel, MessageReceivedLogContext context, Exception exception, long elapsedMilliseconds)
        {
            _logger.LogError(
                "RabbitMQ: Message processing failed, {@details}", new
                {
                    RabbitMessageBrokerClientMessageProcessingFailed = new
                    {
                        topic = context.Topic,
                        subscriptionId = context.SubscriptionId,
                        correlationId = context.CorrelationId,
                        exception,
                        elapsedMilliseconds
                    }
                }
            );
        }

        public void PublishedSucceeded(string topic, string subscriptionId)
        {
            _logger.LogInformation(
                "RabbitMQ: Message published for topic, {@details}", new
                {
                    MessageBrokerClientMessagePublished = new
                    {
                        topic,
                        subscriptionId
                    }
                }
            );
        }

        public void UnsubFromTopic(string topic, string subscriptionId)
        {
            _logger.LogInformation("Unsubscribed from topic. {@details}", new
            {
                MessageBrokerClientUnsubscribedFromTopic = new
                {
                    topic,
                    subscriptionId
                }
            });
        }

        public void SubedAndCompeting(string topic, string subscriptionId)
        {
            _logger.LogInformation("RabbitMQ: Subscribed to topic and competing in worker pool, {@details}", new
            {
                MessageBrokerClientSubscribedToTopic = new
                {
                    topic,
                    subscriptionId
                }
            });
        }

        public void ConnectionLostReconnecting(string topic, string subscriptionId)
        {
            _logger.LogWarning("RabbitMQ: Connection to message broker lost. Attempting to reconnect {@details}", new
            {
                MessageBrokerClientConnectionLost = new
                {
                    topic,
                    subscriptionId
                }
            });
        }

        public void UnableToConnect(string topic,
            string subscriptionId, 
            int attempt, 
            int retryDelay, 
            string host,
            int port)
        {
            _logger.LogWarning("RabbitMQ: Could not establish connection to message broker. {@details}", new
            {
                MessageBrokerClientConnectionError = new
                {
                    topic,
                    subscriptionId,
                    attempt,
                    retryDelayInMilliseconds = retryDelay,
                    host = host,
                    port = port
                }
            });
        }
    }
}
