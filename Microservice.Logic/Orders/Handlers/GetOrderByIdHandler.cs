using MediatR;
using Microservice.Db;
using Microservice.Logic.Mappers;
using Microservice.Logic.Orders.Queries;
using Microservice.Logic.Orders.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;

        public GetOrderByIdHandler(MicroserviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders.FindAsync(request.Id);
            var response = entity?.ToResponse();
            return response;
        }
    }
}
