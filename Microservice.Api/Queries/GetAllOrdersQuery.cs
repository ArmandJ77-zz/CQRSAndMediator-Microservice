using MediatR;
using Microservice.Api.Responses;
using System.Collections.Generic;

namespace Microservice.Api.Queries
{
    public class GetAllOrdersQuery: IRequest<List<OrderResponse>>
    {
    }
}
