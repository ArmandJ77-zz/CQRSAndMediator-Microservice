using MediatR;
using Microservice.Db;
using Microservice.Logic.Orders.Queries;
using Microservice.Logic.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class GetAllOrdersHandler: IRequestHandler<GetAllOrdersQuery,List<OrderResponse>>
    {
        private readonly MicroserviceDbContext _dbContext;

        public GetAllOrdersHandler(MicroserviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.Orders.ToListAsync(cancellationToken: cancellationToken);

            var responses = entities.Select(x => x.ToResponse()).ToList();

            return responses;
        }
    }
}
