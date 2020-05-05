using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Commands
{
    public class PutOrderPlacedCommand : IRequest<OrderResponse>
    {
        public long Id { get; }
        public int Quantity { get; }
        public long PersonId { get; }

        public PutOrderPlacedCommand(long id, long personId, int quantity)
        {
            PersonId = personId;
            Quantity = quantity;
            Id = id;
        }
    }
}
