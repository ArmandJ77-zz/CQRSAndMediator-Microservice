using Microservice.MessageBus.Orders.Events;
using Microservice.RabbitMessageBroker;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Api.MessageBus.OrderSubscriptions.Subscriptions
{
    public class OrderPlacedSubscription : IHostedService
    {
        private readonly IRabbitMessageBrokerClient _messageBrokerClient;
        private readonly IServiceProvider _serviceProvider;
        public OrderPlacedSubscription(IRabbitMessageBrokerClient messageBrokerClient, IServiceProvider serviceProvider)
        {
            _messageBrokerClient = messageBrokerClient;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _messageBrokerClient.Subscribe<OrderPlacedSubscriptionEvent>("OrderPlaced", "Orders", HandleMessage, c => c.UseBasicQos());
        }

        private async Task HandleMessage(OrderPlacedSubscriptionEvent orderPlacedSubscriptionEvent)
        {
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var serviceProvider = scope.ServiceProvider;
            //    var mergeSellerLeadsForPeopleManager = serviceProvider.GetService<IMergeSellerLeadsForPeopleManager>();
            //    await _backgroundProcessingClient.Run(
            //        () => mergeSellerLeadsForPeopleManager.Merge( 
            //            message.NewPersonId,
            //            message.OldPersonId,
            //            message.ActionedById));
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
