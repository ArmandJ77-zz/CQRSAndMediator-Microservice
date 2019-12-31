using System.Collections.Generic;
namespace Microservice.Api.Filters
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
