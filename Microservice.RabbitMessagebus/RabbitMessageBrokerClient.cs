using Microservice.RabbitMessageBroker.Builders;
using Microservice.RabbitMessageBroker.Configuration;
using Microservice.RabbitMessageBroker.Extensions;
using Microservice.RabbitMessageBroker.Logger;
using Microservice.RabbitMessageBroker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.RabbitMessageBroker
{
    public class RabbitMessageBrokerClient : IRabbitMessageBrokerClient
    {
        private IModel _channel { get; set; }

        private readonly ILogger<RabbitMessageBrokerClient> _logger;
        private readonly IOptions<RabbitMessageBrokerSettings> _options;
        private readonly IMessageBrokerDefaultLogger _defaultLogger;
        public RabbitMessageBrokerClient(ILogger<RabbitMessageBrokerClient> logger, IOptions<RabbitMessageBrokerSettings> options)
        {
            _logger = logger;
            _options = options;
            _defaultLogger = new MessageBrokerDefaultLogger(_logger);
        }

        protected async Task<IModel> TryConnect(string topic,
            string subscriptionId = null,
            int attempt = 1,
            int previousRetryDelay = 1000)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.Value.Host,
                Port = _options.Value.Port,
                UserName = _options.Value.UserName,
                Password = _options.Value.Password,
                VirtualHost = _options.Value.VirtualHost ?? "/"
            };

            try
            {
                var connection = factory.CreateConnection();
                return connection.CreateModel();
            }
            catch (BrokerUnreachableException)
            {
                if (attempt > _options.Value.MaxConnectionRetries)
                    throw;

                var retryDelay = _options.Value.ConnectionAttempMaxBackoff;
                if (previousRetryDelay < _options.Value.ConnectionAttempMaxBackoff)
                {
                    var random = new Random(previousRetryDelay);
                    var entropy = random.NextDouble();
                    var retryDelayMax = previousRetryDelay * _options.Value.ConnectionAttempBackoffFactor;
                    var retryDelayDiff = retryDelayMax - previousRetryDelay;
                    retryDelay = Convert.ToInt32(previousRetryDelay + retryDelayDiff * entropy);
                }

                _defaultLogger.UnableToConnect(topic, subscriptionId, attempt, retryDelay, _options.Value.Host, _options.Value.Port);

                await Task.Delay(retryDelay);
                return await TryConnect(topic, subscriptionId, attempt + 1, retryDelay);
            }
        }

        protected async Task<IModel> GetChannel(string topic, string subscriptionId = null)
        {
            if (_options.Value == null)
                throw new Exception("RabbitMQ: Message broker client credentials not configured {@details}");

            if (_channel == null || _channel.IsClosed)
                _channel = await TryConnect(topic, subscriptionId);

            return _channel;
        }

        public Task<Action> Subscribe<T>(
            string topic,
            string subscriptionId,
            Func<T, Task> onReceive,
            Func<MessageBrokerConfigBuilder, MessageBrokerConfigBuilder> configure = null)
        {
            return Subscribe(topic, subscriptionId, eventModel => onReceive((T)eventModel), typeof(T), configure);
        }

        public async Task<Action> Subscribe(string topic,
            string subscriptionId,
            Func<object, Task> onReceive,
            Type eventModelType,
            Func<MessageBrokerConfigBuilder, MessageBrokerConfigBuilder> config = null)
        {
            var brokerConfig = new MessageBrokerConsumerConfig();
            if (config != null)
            {
                var configBuilder = config.Invoke(new MessageBrokerConfigBuilder());
                brokerConfig = configBuilder.Config;
            }

            var channel = await GetChannel(topic, subscriptionId);

            channel.ExchangeDeclare(topic,
                "topic",
                durable: true
            );

            var queueName = $"{topic}_{subscriptionId}";

            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            if (brokerConfig.UseBasicQos)
                channel.BasicQos(0, brokerConfig.BasicQosPrefetch, false);

            channel.QueueBind(
                queueName,
                topic,
                ""
            );

            IMessageBrokerDefaultLogger logger;
            logger = new MessageBrokerDefaultLogger(_logger);
            if (brokerConfig.SubscriptionLogger != null)
                logger = brokerConfig.SubscriptionLogger;

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, basicAckEventArgs) =>
            {
                var sw = new Stopwatch();
                sw.Start();
                var body = Encoding.UTF8.GetString(basicAckEventArgs.Body);
                var instanceModel = new
                {
                    body,
                    DateTime.UtcNow,
                    basicAckEventArgs.ConsumerTag
                };

                var instanceModelJson = JsonConvert.SerializeObject(instanceModel);
                var instanceHash = instanceModelJson.ToMd5Hash();
                var eventModel = JsonConvert.DeserializeObject(body, eventModelType);
                var logContext = new MessageReceivedLogContext
                {
                    CorrelationId = instanceHash,
                    SubscriptionId = subscriptionId,
                    Topic = topic
                };

                logger.MessageReceived(eventModel, logContext, sw.ElapsedMilliseconds);
                sw.Restart();
                try
                {
                    onReceive(eventModel).Wait();

                    logger.ProcessingSucceeded(eventModel, logContext, sw.ElapsedMilliseconds);
                }
                catch (Exception exception)
                {
                    logger.ProcessingFailed(eventModel, logContext, exception, sw.ElapsedMilliseconds);
                    sw.Restart();
                }
            };

            channel.BasicConsume(queueName, true, consumer);

            EventHandler<ShutdownEventArgs> onDisconnect = null;
            onDisconnect = async (sender, args) =>
            {
                channel.ModelShutdown -= onDisconnect;
                if (channel.IsClosed
                    && channel.CloseReason.ReplyCode == 0
                    && channel.CloseReason.ReplyText == "Shutdown")
                    return;

                _defaultLogger.ConnectionLostReconnecting(topic, subscriptionId);
                await Subscribe(topic, subscriptionId, onReceive);
            };

            channel.ModelShutdown += onDisconnect;

            _defaultLogger.SubedAndCompeting(topic, subscriptionId);

            return () =>
            {
                channel.Close(0, "Shutdown");
                _defaultLogger.UnsubFromTopic(topic, subscriptionId);
            };
        }

        public async Task<bool> Publish<T>(string topic, T message)
        {
            var tcs = new TaskCompletionSource<bool>();
            var connectionTimeoutTask = Task.Delay(_options.Value.PublishConnectionTimeoutInSeconds * 1000);
            var connectionTask = GetChannel(topic);

            Task.WaitAny(connectionTask, connectionTimeoutTask);

            if (!connectionTask.IsCompleted)
                throw new Exception("Could not establish connection to message broker in the allowed timeout.");

            var channel = connectionTask.Result;

            channel.ExchangeDeclare(topic,
                "topic",
                durable: true
            );

            var bodyAsString = JsonConvert.SerializeObject(message, Formatting.Indented);
            var body = Encoding.UTF8.GetBytes(bodyAsString);

            channel.BasicPublish(topic,
                "",
                mandatory: true,
                body: body
            );

            _defaultLogger.PublishedSucceeded(topic, bodyAsString);

            tcs.SetResult(true);
            return await tcs.Task;
        }
    }
}
