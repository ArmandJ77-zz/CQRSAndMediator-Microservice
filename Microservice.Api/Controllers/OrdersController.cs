using MediatR;
using Microservice.Logic.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microservice.Logic.Commands;
using Microservice.Logic.Queries;

namespace Microservice.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(long orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { orderId = result.Id }, result);
        }

        [HttpPatch("update/{id}/{personId}")]
        public async Task<IActionResult> PatchOrder([FromRoute]long id,[FromRoute] long personId, [FromBody] JsonPatchDocument<OrderModel> patchModel)
        {
            var command = new PatchOrderCommand {JsonPatchDocument = patchModel, OrderId = id, PersonId = personId};
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
