using System;

namespace Microservice.RabbitMessageBroker.Integration.Tests
{
    public static  class TestObjectMother
    {
        public static TestObject GetBasicTestObject()
        {
            return new TestObject
            {
                Age = 27,
                DateOfBirth = new DateTime(1945),
                Name = "Winston Churchill"
            };
        }
    }
}
