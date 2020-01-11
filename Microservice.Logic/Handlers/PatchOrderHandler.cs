using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Commands;
using Microservice.Logic.Mappers;
using Microservice.Logic.Responses;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Logic.Handlers
{
    public class PatchOrderHandler : BaseHandler, IRequestHandler<PatchOrderCommand, OrderResponse>
    {
        public PatchOrderHandler(MicroserviceDbContext dbContext, IMediator mediator) : base(dbContext, mediator)
        {
        }

        public async Task<OrderResponse> Handle(PatchOrderCommand command, CancellationToken cancellationToken)
        {
            var originalEntity =
                await _dbContext.Orders
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == command.OrderId,
                        cancellationToken: cancellationToken);

            if (originalEntity == null)
                return null;

            var model = originalEntity.ToPatchModal();

            command.JsonPatchDocument.ApplyTo(model, error =>
            {
                Debug.WriteLine($"Failed to apply patch: this is where you add your logger");
            });

            //business rule example that all updated product should be suffixed with PROD if it had 'product' in the name and the new name does not
            if (originalEntity.Name.ToLower().Contains("product") && !model.Name.ToLower().Contains("product"))
            {
                model.Name = $"PROD: {model.Name}";
            }

            var updatedEntity = new Order
            {
                Id = originalEntity.Id,
                Name = model.Name
            }; 

            _dbContext.Update(updatedEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = updatedEntity.ToResponse();
            return response;
        }
    }
}
