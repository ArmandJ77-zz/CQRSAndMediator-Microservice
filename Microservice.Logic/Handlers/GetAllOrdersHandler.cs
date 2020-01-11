using MediatR;
using Microservice.Db;
using Microservice.Logic.Mappers;
using Microservice.Logic.Queries;
using Microservice.Logic.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Handlers
{
    public class GetAllOrdersHandler: BaseHandler,IRequestHandler<GetAllOrdersQuery,List<OrderResponse>>
    {
        public GetAllOrdersHandler(MicroserviceDbContext dbContext, IMediator mediator) : base(dbContext, mediator)
        {
        }

        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.Orders.ToListAsync(cancellationToken: cancellationToken);

            var responses = entities.Select(x => x.ToResponse()).ToList();

            return responses;
        }
    }
}
