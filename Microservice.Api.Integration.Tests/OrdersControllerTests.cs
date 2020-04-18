using Microservice.Api.Integration.Tests.Infrastructure;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Responses;
using Microservice.RabbitMessageBroker.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        [Test]
        public async Task Given_CreateOrder_Publish_OrderCreatedEvent_Expect_OrderCreatedEvent_From_MessageBroker()
        {

        }
//        [Test]
//        public async Task RabbitMQFanOutPublishTest()
//        {
//            var settings = new RabbitMessageBrokerSettings
//            {
//                Host = "localhost",
//                Port = 15672,
//                Password = "guest",
//                UserName = "guest"
//            };
//
//            var factory = new ConnectionFactory() { HostName = "localhost" };
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
//
//                var message = GetMessage();
//                var body = Encoding.UTF8.GetBytes(message);
//                channel.BasicPublish(exchange: "logs",
//                    routingKey: "",
//                    basicProperties: null,
//                    body: body);
//                Console.WriteLine(" [x] Sent {0}", message);
//            }
//
//            var subfactory = new ConnectionFactory() { HostName = "localhost" };
//            using (var connection = subfactory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
//
//                var queueName = channel.QueueDeclare().QueueName;
//                channel.QueueBind(queue: queueName,
//                    exchange: "logs",
//                    routingKey: "");
//
//                Console.WriteLine(" [*] Waiting for logs.");
//
//                var consumer = new EventingBasicConsumer(channel);
//                consumer.Received += (model, ea) =>
//                {
//                    var body = ea.Body;
//                    var message = Encoding.UTF8.GetString(body);
//                    Console.WriteLine(" [x] {0}", message);
//                };
//                channel.BasicConsume(queue: queueName,
//                    autoAck: true,
//                    consumer: consumer);
//
//                Console.WriteLine(" Press [enter] to exit.");
//                Console.ReadLine();
//            }
//        }
//
//        [Test]
//        public async Task RabbitMQDirectPublishTest()
//        {
//            var factory = new ConnectionFactory() { HostName = "localhost" };
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.QueueDeclare(queue: "hello",
//                    durable: false,
//                    exclusive: false,
//                    autoDelete: false,
//                    arguments: null);
//
//                string message = "Hello World!";
//                var body = Encoding.UTF8.GetBytes(message);
//
//                channel.BasicPublish(exchange: "",
//                    routingKey: "hello",
//                    basicProperties: null,
//                    body: body);
//
//            }
//        }
//
//        [Test]
//        public async Task RabbitMQDirectSubscribeTest()
//        {
//            var factory = new ConnectionFactory() { HostName = "localhost" };
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.QueueDeclare(queue: "hello",
//                    durable: false,
//                    exclusive: false,
//                    autoDelete: false,
//                    arguments: null);
//
//                var consumer = new EventingBasicConsumer(channel);
//                consumer.Received += (model, ea) =>
//                {
//                    var body = ea.Body;
//                    var message = Encoding.UTF8.GetString(body);
//                    Console.WriteLine(" [x] Received {0}", message);
//                };
//                channel.BasicConsume(queue: "hello",
//                    autoAck: true,
//                    consumer: consumer);
//
//                Console.WriteLine(" Press [enter] to exit.");
//                Console.ReadLine();
//            }
//        }

        private static string GetMessage()
        {
            return "info: Hello World!";
        }
    }
}
