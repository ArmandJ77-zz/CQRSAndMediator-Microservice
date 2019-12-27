using MediatR;
using Microservice.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microservice.Logic.Mappers;
using Microservice.Logic.Queries;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Handlers
{
    public class GetAllOrdersHandler: BaseHandler,IRequestHandler<GetAllOrdersQuery,List<OrderResponse>>
    {
        public GetAllOrdersHandler(MicroserviceDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.Orders.ToListAsync(cancellationToken: cancellationToken);
            var response = _mapper.MapOrderEntitiesToOrderResponses(entities);

            return response;

        }
    }
}
