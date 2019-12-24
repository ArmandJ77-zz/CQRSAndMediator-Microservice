using MediatR;
using Microservice.Api.Database;
using Microservice.Api.Mappers;
using Microservice.Api.Queries;
using Microservice.Api.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Api.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(MicroserviceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders.FindAsync(request.Id);
            var response = _mapper.MapOrderEntityToOrderResponse(entity);
            return response;
        }
    }
}
