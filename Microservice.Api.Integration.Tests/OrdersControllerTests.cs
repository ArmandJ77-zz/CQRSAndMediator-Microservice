using Microservice.Api.Integration.Tests.Infrastructure;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Events;
using Microservice.Logic.Orders.Responses;
using Microservice.RabbitMessageBroker;
using Microservice.RabbitMessageBroker.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice.Api.Integration.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private IRabbitMessageBrokerClient _messageBrokerClient;

        private string PathBuilder(string extension) => $"api/{extension}";

        [OneTimeSetUp]
        public void Setup()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var config = configBuilder.Build();

            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions));
                        services.RemoveAll(typeof(MicroserviceDbContext));
                        services.AddDbContext<MicroserviceDbContext>();
                        services.AddRabbitMqMessageBroker(config.GetSection("RabbitMessageBrokerSettings"));
                    });
                });

            _client = _factory.CreateClient();
            _messageBrokerClient = _factory.Services.GetService<IRabbitMessageBrokerClient>();
        }

        [Test]
        public async Task Given_CreateOrderCommand_Which_Should_Publish_A_OrderCreatedEvent_Expect_OrderResponse_With_CreatedEvent_Published()
        {
            // Arrange
            const string topic = "OrderCreated";
            const string subscriptionId = "OrderCreated_IntegrationTest";
            var createCommand = new CreateOrderCommand("Keyboard", 5);

            _factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
            });

            // Act
            var response = await _client.PostAsJsonAsync(PathBuilder("orders"), createCommand);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var result = response.Content.Deserialize<OrderResponse>().Result;
            var subscriptionResponse = await _messageBrokerClient.Subscribe<OrderCreatedEvent>(
                topic,
                subscriptionId,
                AssertCallback,
                c => c.UseBasicQos());

            // Assert Create Response
            Assert.That(result.Quantity, Is.EqualTo(createCommand.Quantity));
            StringAssert.AreEqualIgnoringCase(createCommand.Name, result.Name);

            // Assert Messagebus OrderCreatedEvent
            Task AssertCallback(OrderCreatedEvent orderCreatedEvent)
            {
                var tcs = new TaskCompletionSource<OrderCreatedEvent>();

                StringAssert.AreEqualIgnoringCase(orderCreatedEvent.Name, result.Name);
                Assert.That(orderCreatedEvent.Quantity, Is.EqualTo(createCommand.Quantity));
                Assert.That(orderCreatedEvent.Id, Is.GreaterThan(0));
                tcs.SetResult(orderCreatedEvent);
                return tcs.Task;
            }
        }

        [Test]
        public async Task Given_GetOrderById_Expect_OrderResponse()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                Name = "Testing command"
            };

            _factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(order);
            });

            // Act
            var response = await _client.GetAsync(PathBuilder($"orders/{order.Id}"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<OrderResponse>().Result;

            // Assert
            StringAssert.AreEqualIgnoringCase(order.Name, result.Name);
        }

        [Test]
        public async Task Given_AllOrders_Expect_OrderResponses()
        {
            // Arrange
            var order1 = new Order
            {
                Id = 1,
                Name = "Testing command"
            };

            var order2 = new Order
            {
                Id = 2,
                Name = "Testing command"
            };


            var order3 = new Order
            {
                Id = 3,
                Name = "Testing command"
            };

            _factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.AddRange(order1, order2, order3);
            });

            var response = await _client.GetAsync(PathBuilder($"orders"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<List<OrderResponse>>().Result;

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task Given_PatchOrder_Which_Should_Publish_OrderUpdatedEvent_Expect_Updated_OrderResponse_With_UpdatedEvent_Published()
        {
            // Arrange
            const string topic = "Order";
            const string subscriptionId = "OrderUpdated";
            var originationOrder = new Order
            {
                Id = 1,
                Name = "product zero one"
            };

            var patchDoc = new JsonPatchDocument<OrderResponse>();
            patchDoc.Replace(x => x.Name, "zero one");

            var operations = patchDoc.Operations.ToList();

            _factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(originationOrder);
            });

            // Act
            var response = await _client.PatchAsJsonAsync(PathBuilder($"orders/update/{originationOrder.Id}/77"), patchDoc);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<OrderResponse>().Result;
            var subscriptionResponse = await _messageBrokerClient.Subscribe<OrderUpdatedEvent>(
                topic,
                subscriptionId,
                AssertCallback,
                c => c.UseBasicQos());

            // Assert Patch Operation
            StringAssert.AreEqualIgnoringCase($"PROD: zero one", result.Name);

            // Assert OrderUpdatedEvent
            Task AssertCallback(OrderUpdatedEvent updatedEvent)
            {
                var tcs = new TaskCompletionSource<OrderUpdatedEvent>();

                StringAssert.AreEqualIgnoringCase($"PROD: zero one", updatedEvent.Name);
                Assert.That(updatedEvent.Quantity, Is.EqualTo(result.Quantity));
                Assert.That(updatedEvent.Id, Is.EqualTo(result.Id));

                tcs.SetResult(updatedEvent);
                return tcs.Task;
            }

        }
    }
}
