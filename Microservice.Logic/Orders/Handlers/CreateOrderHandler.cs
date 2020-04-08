using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Mappers;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class CreateOrderHandler: IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;

        public CreateOrderHandler(MicroserviceDbContext dbContext)
        {
            _dbContext = dbContext;
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
