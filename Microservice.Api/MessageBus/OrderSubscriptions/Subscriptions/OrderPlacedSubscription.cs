using MediatR;
using Microservice.RabbitMessageBroker;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Api.MessageBus.OrderSubscriptions.Subscriptions
{
    public class OrderPlacedSubscription: IHostedService
    {
        private readonly IRabbitMessageBrokerClient _messageBrokerClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        public OrderPlacedSubscription(IRabbitMessageBrokerClient messageBrokerClient, IServiceProvider serviceProvider, IMediator mediator)
        {
            _messageBrokerClient = messageBrokerClient;
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _mediator.
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
