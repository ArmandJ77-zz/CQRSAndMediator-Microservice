using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Models;
using Microservice.Logic.Orders.Queries;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> PatchOrder([FromRoute]long id,[FromRoute] long personId, [FromBody] JsonPatchDocument<OrderPatchModel> patchModel)
        {
            var command = new PatchOrderCommand(id,personId,patchModel);
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
