using MediatR;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.EventPublishers;
using Microservice.Logic.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class PatchOrderHandler : IRequestHandler<PatchOrderCommand, OrderResponse>
    {
        private readonly MicroserviceDbContext _dbContext;
        private readonly IOrderPatchedEventPublisher _updatedEventPublisher;

        public PatchOrderHandler(MicroserviceDbContext dbContext, IOrderPatchedEventPublisher updatedEventPublisher)
        {
            _dbContext = dbContext;
            _updatedEventPublisher = updatedEventPublisher;
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

            await _updatedEventPublisher.Publish(updatedEntity.ToUpdatedEvent());

            var response = updatedEntity.ToResponse();
            return response;
        }
    }
}
