using System.IO;
using System.Threading.Tasks;
using Microservice.RabbitMessageBroker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Microservice.RabbitMessageBroker.Integration.Tests
{
    [TestFixture]
    public class RabbitMessageBusTests
    {
        private IRabbitMessageBrokerClient MessageBrokerClient { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var config = configBuilder.Build();

            var services = new ServiceCollection();

            services
                .AddRabbitMqMessageBroker(config.GetSection("RabbitMessageBrokerSettings"))
                .AddLogging();

            MessageBrokerClient = services.BuildServiceProvider().GetService<IRabbitMessageBrokerClient>();
        }

        [Test]
        public async Task Given_Subscribe_TestMessageTopic_Expect_HelloWorld_Response()
        {
            const string topic = "TestMessageTopic";
            const string subscriptionId = "SubscribeToTestMessage";
            const string message = "Hello World";
            var result = await MessageBrokerClient.Publish<string>(topic, message);

            var subscriptionResponse = await MessageBrokerClient.Subscribe<string>(topic,
                subscriptionId, AssertCallback, c => c.UseBasicQos());

            static Task AssertCallback(string messageEvent)
            {
                var tcs = new TaskCompletionSource<string>();
                StringAssert.AreEqualIgnoringCase("HelloWorld", messageEvent);
                tcs.SetResult(messageEvent);
                return tcs.Task;
            }
        }

        [Test]
        public async Task Given_Subscribe_TestObjectTopic_Expect_TestObject_Response()
        {
            const string topic = "TestObjectTopic";
            const string subscriptionId = "SubscribeToTestObject";
            var message = TestObjectMother.GetBasicTestObject();

            var result = await MessageBrokerClient.Publish(topic, message);
            Assert.That(result);

            var subscriptionResponse = await MessageBrokerClient.Subscribe<TestObject>(topic,
                subscriptionId, AssertCallback, c => c.UseBasicQos());

            static Task AssertCallback(TestObject messageEvent)
            {
                var tcs = new TaskCompletionSource<bool>();
                Assert.That(TestObjectMother.GetBasicTestObject().Age, Is.EqualTo(messageEvent.Age));
                Assert.That(TestObjectMother.GetBasicTestObject().DateOfBirth, Is.EqualTo(messageEvent.DateOfBirth));
                StringAssert.AreEqualIgnoringCase(TestObjectMother.GetBasicTestObject().Name, messageEvent.Name);
                tcs.SetResult(true);
                return tcs.Task;
            }
        }
    }
}
