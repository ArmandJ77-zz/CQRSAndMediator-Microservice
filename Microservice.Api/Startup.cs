using FluentValidation.AspNetCore;
using MediatR;
using Microservice.Api.Filters;
using Microservice.Db.Configuration;
using Microservice.HanfireWithRedisBackingStore.Configuration;
using Microservice.Logic.Configuration;
using Microservice.Logic.Orders.Validators;
using Microservice.RabbitMessageBroker.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservice.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddControllers()
                .AddNewtonsoftJson()
                ;

            services
                .AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateOrderValidator>())
                ;

            services
                .AddDatabase(_configuration.GetConnectionString("Database"))
                .AddLogic(_configuration)
                .AddMediatR(typeof(LogicServiceCollectionExtensions).Assembly)
                //.AddMessageBroker(_configuration.GetSection("MessageBrokerSettings"))
                //.AddBackgroundJobServer(_configuration.GetSection("BackgroundJobServerSettings"))
                ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
