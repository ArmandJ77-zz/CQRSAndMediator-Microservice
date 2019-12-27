using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservice.Db;
using Microservice.Logic.Mappers;
using Microservice.Logic.Queries;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Handlers
{
    public class GetOrderByIdHandler : BaseHandler,IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        public GetOrderByIdHandler(MicroserviceDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders.FindAsync(request.Id);
            
            if (entity == null)
                return null;
            
            var response = _mapper.MapOrderEntityToOrderResponse(entity);
           
            return response;
        }
    }
}
