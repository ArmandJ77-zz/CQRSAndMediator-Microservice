using System.Collections.Generic;

namespace Microservice.HanfireWithRedisBackingStore.Configuration
{
    public class BackgroundJobServerSettings
    {
        public string RedisConnectionString { get; set; }
        public int RedisDatabaseNumber { get; set; }
        public int? NumberOfWorkers { get; set; }
        public int? RetryAttempts { get; set; }
        public List<string> Queues { get; set; }
        public string DashboardEndpoint { get; set; }
    }
}
