using MediatR;
using Microservice.Api.Commands;
using Microservice.Api.Database;
using Microservice.Api.Database.EntityModels;
using Microservice.Api.Mappers;
using Microservice.Api.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Api.Handlers
{
    public class CreateCustomerOrderHandler: IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateCustomerOrderHandler(MicroserviceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
