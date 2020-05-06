using Microservice.Db;
using Microservice.RabbitMessageBroker;
using Microservice.RabbitMessageBroker.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System.Net.Http;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public class BaseControllerTest
    {
        protected internal WebApplicationFactory<Startup> Factory;
        protected internal HttpClient Client;
        protected internal IRabbitMessageBrokerClient MessageBrokerClient;
        public string BaseRoute { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            Factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions));
                        services.RemoveAll(typeof(MicroserviceDbContext));
                        services.AddDbContext<MicroserviceDbContext>();
                        services.AddRabbitMqMessageBroker(ConfigurationBuilderExtensions.GetCustomSection("RabbitMessageBrokerSettings"));
                    });
                });

            Client = Factory.CreateClient();
            MessageBrokerClient = Factory.Services.GetService<IRabbitMessageBrokerClient>();
        }

        public  string GetBaseRoute() => $"api/{BaseRoute}";
    }
}
