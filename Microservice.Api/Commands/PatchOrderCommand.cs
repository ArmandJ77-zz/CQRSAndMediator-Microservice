using MediatR;
using Microservice.Api.Model;
using Microservice.Api.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace Microservice.Api.Commands
{
    public class PatchOrderCommand : IRequest<OrderResponse>
    {
        public long OrderId { get; set; }
        public long PersonId { get; set; }
        public JsonPatchDocument<OrderModel> JsonPatchDocument { get; set; }
    }
}
