using Hangfire;
using Microservice.HangfireBackgroundJobServer;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microservice.Logic.BackgroundProcessing
{
    public class HeartbeatRecurringJob : IRecurringJob
    {
        private readonly ILogger<HeartbeatRecurringJob> _logger;
        public HeartbeatRecurringJob(ILogger<HeartbeatRecurringJob> logger)
        {
            _logger = logger;
        }

        public Task Run(IJobCancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();

            _logger.LogInformation("Heartbeat completed, {@details}", new
            {
                HeartbeatCompleted = new
                {
                    Took = sw.ElapsedMilliseconds
                }
            });

            return Task.CompletedTask;
        }
    }
}
