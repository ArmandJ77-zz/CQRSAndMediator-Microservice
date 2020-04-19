using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Logic.Configuration
{
    public static class LogicServiceCollectionExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection container, IConfiguration config)
        {
            var settings = new MicroserviceSettings();
            config.Bind(settings);

            container
                //Domain Level Validation
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>))
                ;

            return container;
        }
    }
}
