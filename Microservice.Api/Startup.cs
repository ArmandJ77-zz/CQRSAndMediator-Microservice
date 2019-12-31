using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microservice.Api.Filters;
using Microservice.Db;
using Microservice.Logic.Config;
using Microservice.Logic.PipelineBehaviours;
using Microservice.Logic.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

        // This method gets called by the runtime. Use this method to add services to the container.
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
                })
                ;
            services.AddDbContext<MicroserviceDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MicroserviceDbContext")));

            services.ConfigureLogic(Configuration);

            services.AddMediatR(typeof(ConfigureServiceCollectionExtensions).Assembly);
            //Domain Level Validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
