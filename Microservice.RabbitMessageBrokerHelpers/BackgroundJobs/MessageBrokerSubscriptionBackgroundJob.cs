using Microservice.HangfireBackgroundJobServer.Infrastructure;
using Microservice.RabbitMessageBrokerHelpers.Builders;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using Microservice.RabbitMessageBrokerHelpers.Models;
using Microservice.RabbitMQMessageBrokerExtension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.BackgroundJobs
{
    public class MessageBrokerSubscriptionBackgroundJob : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBackgroundProcessingClient _backgroundProcessingClient;
        private readonly IRabbitMessageBrokerClient _messageBrokerClient;
        private readonly ILogger<MessageBrokerSubscriptionBackgroundJob> _logger;
        private readonly MessageBrokerSubscriptionsConfigurationBuilder _configurationBuilder;
        private List<Action> _unsubscribeCallbacks;

        public MessageBrokerSubscriptionBackgroundJob(
            MessageBrokerSubscriptionsConfigurationBuilder configurationBuilder,
            IServiceProvider serviceProvider,
            IRabbitMessageBrokerClient messageBrokerClient,
            ILogger<MessageBrokerSubscriptionBackgroundJob> logger,
            IBackgroundProcessingClient backgroundProcessingClient)
        {
            _configurationBuilder = configurationBuilder;
            _serviceProvider = serviceProvider;
            _messageBrokerClient = messageBrokerClient;
            _logger = logger;
            _backgroundProcessingClient = backgroundProcessingClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var tasks = _configurationBuilder.Subscriptions
                .Select(subscription => _messageBrokerClient.Subscribe(
                    subscription.Topic,
                    subscription.Pool,
                    eventModel => Handle(eventModel, subscription),
                    subscription.EventModel))
                .ToList();

            await Task.WhenAll(tasks);

            _unsubscribeCallbacks = tasks.Select(t => t.Result).ToList();
        }

        public async Task Handle(object eventModel, MessageBrokerSubscription subscription)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService(subscription.Handler) as IMessageBrokerSubscriptionHandler;
                if (handler == null)
                {
                    _logger.LogError("Could not resolve subscription handler. {@details}", new
                    {
                        CouldNotResolveIntegrationEventHandler = new
                        {
                            subscription
                        }
                    });
                }

                await _backgroundProcessingClient.Run(() => handler.HandleEventModel(eventModel));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var unsubscribeCallback in _unsubscribeCallbacks)
            {
                unsubscribeCallback();
            }

            return Task.CompletedTask;
        }
    }
}
