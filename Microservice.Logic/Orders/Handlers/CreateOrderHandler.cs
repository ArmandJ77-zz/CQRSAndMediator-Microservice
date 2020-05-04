using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.EventPublishers;
using Microservice.Logic.Orders.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IOrderCreatedEventPublisher _createdEventPublisher;

        public CreateOrderHandler(MicroserviceDbContext dbContext, IOrderCreatedEventPublisher createdEventPublisher)
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
