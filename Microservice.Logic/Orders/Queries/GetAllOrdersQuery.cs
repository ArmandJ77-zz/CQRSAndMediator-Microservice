using System.Collections.Generic;
using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Queries
{
    public class GetAllOrdersQuery: IRequest<List<OrderResponse>>
    {
    }
}
