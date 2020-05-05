using MediatR;
using Microservice.Db;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.EventPublishers;
using Microservice.Logic.Orders.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class PutOrderPlacedHandler : IRequestHandler<PutOrderPlacedCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IOrderPlacedEventPublisher _orderPlacedEventPublisher;

        public PutOrderPlacedHandler(MicroserviceDbContext dbContext, IOrderPlacedEventPublisher orderPlacedEventPublisher)
        {
            _dbContext = dbContext;
            _orderPlacedEventPublisher = orderPlacedEventPublisher;
        }

        public async Task<OrderResponse> Handle(PutOrderPlacedCommand request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(request.Id);

            if (order == null)
                throw new KeyNotFoundException($"Unable to modify order because an entry with Id: {request.Id} could not be found");

            if (order.Quantity < request.Quantity)
                throw new ArgumentOutOfRangeException($"Unable to place order as the requested quantity ({request.Quantity}) is greater than the in stock quantity ({order.Quantity})");

            var quantityBeforeReduction = order.Quantity;
            order.Quantity -= request.Quantity;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _orderPlacedEventPublisher.Publish(order.ToOrderPlacedEvent(quantityBeforeReduction));
            
            var response = order.ToResponse();

            return response;
        }
    }
}
