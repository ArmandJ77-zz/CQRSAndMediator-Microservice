using MediatR;
using System.Collections.Generic;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Queries
{
    public class GetAllOrdersQuery: IRequest<List<OrderResponse>>
    {
    }
}
