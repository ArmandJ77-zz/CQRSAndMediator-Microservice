using MediatR;
using Microservice.Logic.Model;
using Microservice.Logic.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace Microservice.Logic.Commands
{
    public class PatchOrderCommand : IRequest<OrderResponse>
    {
        public long OrderId { get; set; }
        public long PersonId { get; set; }
        public JsonPatchDocument<OrderModel> JsonPatchDocument { get; set; }
    }
}
