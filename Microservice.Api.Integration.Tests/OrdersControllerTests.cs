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
    public class OrdersControllerTests : BaseControllerTest
    {
        public OrdersControllerTests()
        {
            BaseRoute = "orders";
        }

        [Test]
        public async Task Given_CreateOrderCommand_Which_Should_Publish_A_OrderCreatedEvent_Expect_OrderResponse_With_CreatedEvent_Published()
        {
            // Arrange
            const string topic = "OrderCreated";
            const string subscriptionId = "OrderCreated_IntegrationTest";
            var createCommand = new CreateOrderCommand("Keyboard", 5);

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
            });

            // Act
            var response = await Client.PostAsJsonAsync(GetBaseRoute(), createCommand);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var result = response.Content.Deserialize<OrderResponse>().Result;
            var subscriptionResponse = await MessageBrokerClient.Subscribe<OrderCreatedEvent>(
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

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(order);
            });

            // Act
            var response = await Client.GetAsync($"{GetBaseRoute()}/{order.Id}");
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

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.AddRange(order1, order2, order3);
            });

            var response = await Client.GetAsync($"{GetBaseRoute()}");
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

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(originationOrder);
            });

            // Act
            var response = await Client.PatchAsJsonAsync($"{GetBaseRoute()}/update/{originationOrder.Id}/77", patchDoc);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<OrderResponse>().Result;
            var subscriptionResponse = await MessageBrokerClient.Subscribe<OrderUpdatedEvent>(
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

        [Test]
        public async Task Given_PutOrderPlacedCommand_Which_Should_Publish_A_PutOrderPlacedEvent_Expect_OrderResponse_With_OrderPlacedEvent_Published()
        {
            // Arrange
            const string topic = "OrderPlacedUpdated";
            const string subscriptionId = "OrderPlacedUpdated_IntegrationTest";
            var command = new PutOrderPlacedCommand(1, 5, 2);
            var order = new Order
            {
                Id = 1,
                Name = "product zero one",
                Quantity = 10
            };

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(order);
            });

            // Act
            var response = await Client.PutAsJsonAsync($"{GetBaseRoute()}/place", command);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = response.Content.Deserialize<OrderResponse>().Result;

            var subscriptionResponse = await MessageBrokerClient.Subscribe<OrderPlacedEvent>(
                topic,
                subscriptionId,
                AssertCallback,
                c => c.UseBasicQos());

            // Assert PutOrderPlaced Response
            Assert.That(result.Quantity, Is.EqualTo(8));
            Assert.That(result.Id, Is.EqualTo(order.Id));
            StringAssert.AreEqualIgnoringCase(order.Name, result.Name);

            // Assert Messagebus OrderCreatedEvent
            Task AssertCallback(OrderPlacedEvent orderPlacedEvent)
            {
                var tcs = new TaskCompletionSource<OrderPlacedEvent>();

                Assert.That(orderPlacedEvent.QuantityBeforeReduction, Is.EqualTo(order.Quantity));
                Assert.That(orderPlacedEvent.Quantity, Is.EqualTo(result.Quantity));
                Assert.That(result.Id, Is.EqualTo(order.Id));
                StringAssert.AreEqualIgnoringCase(order.Name, result.Name);

                tcs.SetResult(orderPlacedEvent);
                return tcs.Task;
            }
        }

     
    }
}
