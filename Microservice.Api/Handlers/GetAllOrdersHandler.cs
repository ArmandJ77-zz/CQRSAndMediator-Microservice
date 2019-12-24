using MediatR;
using Microservice.Api.Database;
using Microservice.Api.Mappers;
using Microservice.Api.Queries;
using Microservice.Api.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Api.Handlers
{
    public class GetAllOrdersHandler: IRequestHandler<GetAllOrdersQuery,List<OrderResponse>>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllOrdersHandler(MicroserviceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.Orders.ToListAsync(cancellationToken: cancellationToken);
            var response = _mapper.MapOrderEntitiesToOrderResponses(entities);

            return response;

        }
    }
}
