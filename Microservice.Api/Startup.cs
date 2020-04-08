using FluentValidation.AspNetCore;
using MediatR;
using Microservice.Api.Filters;
using Microservice.Logic.Config;
using Microservice.Logic.Orders.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservice.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Default", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services
                .AddControllers()
                .AddNewtonsoftJson();
            services
                .AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
                // API Level Validation
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<CreateOrderValidator>();
                });

            services.ConfigureLogic(Configuration);

            services.AddMediatR(typeof(ConfigureServiceCollectionExtensions).Assembly);
            //Domain Level Validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

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
