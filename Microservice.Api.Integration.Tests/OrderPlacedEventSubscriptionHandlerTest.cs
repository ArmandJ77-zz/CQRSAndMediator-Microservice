using Microservice.Api.Integration.Tests.Infrastructure;
using Microservice.Db;
using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Events;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Api.Integration.Tests
{
    [TestFixture]
    public class OrderPlacedEventSubscriptionHandlerTest: BaseEventSubscriptionTest
    {
        [Test]
        public async Task Given_OrderPlacedSubscriptionEvent_Expect_OrderResponse_With_OrderPlacedEvent_Published()
        {
            // Arrange
            var order = new Order
            {
                Id = 2,
                Name = "product zero two",
                Quantity = 5
            };

            var orderPlacedSubscriptionEvent = new OrderPlacedSubscriptionEvent
            {
                Id = order.Id,
                Quantity = 2,
                PersonId = 33
            };

            Factory.Seed<Startup, MicroserviceDbContext>(db =>
            {
                db.Clear();
                db.Orders.Add(order);
            });

            // Act
            var handler = MessageBrokerClient.Invocations.
                    First(i => (string)i.Arguments[0] == "OrderPlaced")
                    .Arguments[2] as Func<object, Task>;

            await handler.Invoke(orderPlacedSubscriptionEvent);

            var dbContext = Factory.GetScopedService<Startup, MicroserviceDbContext>();

            var orderResult = dbContext.Orders.Find(order.Id);

            // Assert handler
            Assert.That(orderResult, Is.Not.Null);
            Assert.That(orderResult.Quantity, Is.EqualTo(order.Quantity - orderPlacedSubscriptionEvent.Quantity));
        }
    }
}
