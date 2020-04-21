using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Responses;
using Microservice.MessageBus;
using Microservice.MessageBus.Orders.Publishers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IMessageBusPublisher<OrderCreatedEventPublisher> _createdEventPublisher;

        public CreateOrderHandler(MicroserviceDbContext dbContext, IMessageBusPublisher<OrderCreatedEventPublisher> createdEventPublisher)
        {
            _dbContext = dbContext;
            _createdEventPublisher = createdEventPublisher;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                Name = request.Name
            };

            _dbContext.Orders.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = entity.ToResponse();

            await _createdEventPublisher.Publish(entity.ToCreatedEvent());

            return response;
        }
    }
}
