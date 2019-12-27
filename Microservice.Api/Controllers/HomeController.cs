using System;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController: ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult Health()
        {
            return Ok($"Build #{Environment.GetEnvironmentVariable("VERSION")}");
        }
    }
}
