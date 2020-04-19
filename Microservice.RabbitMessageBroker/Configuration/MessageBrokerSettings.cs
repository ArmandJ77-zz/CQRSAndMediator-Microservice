using Microservice.RabbitMessageBroker.Models;

namespace Microservice.RabbitMessageBroker.Configuration
{
    public  class MessageBrokerSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int MaxConnectionRetries { get; set; } = 1000;
        public double ConnectionAttempBackoffFactor { get; set; } = 1.5;
        public int ConnectionAttempMaxBackoff { get; set; } = 10000;
        public int PublishConnectionTimeoutInSeconds { get; set; } = 10;
    }
}
