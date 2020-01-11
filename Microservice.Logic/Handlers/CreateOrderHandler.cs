using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Commands;
using Microservice.Logic.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microservice.Logic.Mappers;

namespace Microservice.Logic.Handlers
{
    public class CreateOrderHandler: BaseHandler,IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        public CreateOrderHandler(MicroserviceDbContext dbContext, IMediator mediator) : base(dbContext, mediator)
        {
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

            return response;
        }
    }
}
