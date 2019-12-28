using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microservice.Logic.Mappers;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Handlers
{
    public class CreateOrderHandler: BaseHandler,IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        public CreateOrderHandler(MicroserviceDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
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

            var response = _mapper.MapOrderEntityToOrderResponse(entity);

            return response;
        }
    }
}
