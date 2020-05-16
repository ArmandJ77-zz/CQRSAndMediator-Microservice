using Microservice.RabbitMessageBroker;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public class BaseEventSubscriptionTest
    {
        private MockRepository _mockRepository; 
        protected internal Mock<IRabbitMessageBrokerClient> MessageBrokerClient;
        protected internal  WebApplicationFactory<Startup> Factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            MessageBrokerClient = _mockRepository.Create<IRabbitMessageBrokerClient>(MockBehavior.Loose);
            Factory = new WebApplicationFactory<Startup>()
                    
                    .WithWebHostBuilder(x =>
                    {
                        x.ConfigureAppConfiguration((c, b) => { b.AddConfiguration(ConfigurationBuilderExtensions.GetConfigurationRoot()); });
                        x.ConfigureTestServices(s => { s.AddTransient(_ => MessageBrokerClient.Object); });
                    })
                ;
        }
        [TearDown]
        public void TearDown()
        {
            _mockRepository.VerifyAll();
        }
    }
}
