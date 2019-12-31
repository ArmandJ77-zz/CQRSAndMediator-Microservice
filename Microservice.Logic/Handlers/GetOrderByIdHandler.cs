using MediatR;
using Microservice.Db;
using Microservice.Logic.Commands;
using Microservice.Logic.Mappers;
using Microservice.Logic.Queries;
using Microservice.Logic.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Handlers
{
    public class GetOrderByIdHandler : BaseHandler, IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        private readonly IMediator _mediator;
        public GetOrderByIdHandler(MicroserviceDbContext dbContext, IMapper mapper, IMediator mediator)
            : base(dbContext, mapper)
        {
            _mediator = mediator;
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders.FindAsync(request.Id);

            if (entity == null)
                return null;

            var response = _mapper.MapOrderEntityToOrderResponse(entity);
            var foo = await _mediator.Send(new CreateOrderCommand { Name = null }, cancellationToken);
            return response;
        }
    }
}
