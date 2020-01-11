using MediatR;
using Microservice.Db;
using Microservice.Logic.Mappers;
using Microservice.Logic.Queries;
using Microservice.Logic.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Handlers
{
    public class GetOrderByIdHandler : BaseHandler, IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        public GetOrderByIdHandler(MicroserviceDbContext dbContext, IMediator mediator) : base(dbContext, mediator)
        {
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders.FindAsync(request.Id);
            var response = entity?.ToResponse();
            return response;
        }
    }
}
