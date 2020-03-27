using Microservice.Api.Integration.Tests.Infrastructure;
using Microservice.Db;
using Microservice.Logic.Commands;
using Microservice.Logic.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microservice.Db.EntityModels;
using Microsoft.AspNetCore.JsonPatch;

namespace Microservice.Api.Integration.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {

            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions));
                        services.RemoveAll(typeof(MicroserviceDbContext));
                        services.AddDbContext<MicroserviceDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestInMemoryDatabase");
                        });
                    });
                });

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Given_CreateCustomerOrderCommand_Expect_OrderResponse()
        {
            // Arrange
            var createCustomerOrderCommand = new CreateOrderCommand("Testing command");

            _factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
            });

            // Act

            var response = await _client.PostAsJsonAsync("/orders", createCustomerOrderCommand);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var result = response.Content.Deserialize<OrderResponse>().Result;

            // Assert
            StringAssert.AreEqualIgnoringCase(createCustomerOrderCommand.Name, result.Name);
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
            var response = await _client.GetAsync($"/orders/{order.Id}");
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

            var response = await _client.GetAsync($"/orders");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<List<OrderResponse>>().Result;

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task Given_PatchOnOrder_Expact_Updated_OrderResponse()
        {
            // Arrange
            var origionalOrder = new Order
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
                db.Orders.Add(origionalOrder);
            });

            // Act
            var response = await _client.PatchAsJsonAsync($"/orders/update/{origionalOrder.Id}/77", patchDoc);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<OrderResponse>().Result;

            // Assert
            StringAssert.AreEqualIgnoringCase($"PROD: zero one", result.Name);
        }
    }
}
