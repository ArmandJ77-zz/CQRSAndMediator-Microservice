using Microservice.HangfireBackgroundJobServer.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.BackgroundProcessing
{
    public class JobScheduler : IHostedService
    {
        private readonly IBackgroundProcessingClient _backgroundProcessingClient;
        private readonly IOptionsMonitor<JobSettings> _settings;

        public JobScheduler(
            IBackgroundProcessingClient backgroundProcessingClient, 
            IOptionsMonitor<JobSettings> settings)
        {
            _backgroundProcessingClient = backgroundProcessingClient;
            _settings = settings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _backgroundProcessingClient.ConfigureRecurringJob<HeartbeatRecurringJob>(
                "Heartbeat",
                _settings.CurrentValue.Heartbeat.Cron,
                _settings.CurrentValue.Heartbeat.Enabled
            );

            _backgroundProcessingClient.ConfigureRecurringJob<ReStockZeroQuantityItemsRecurringJob>(
                "ReStockZeroQuantityItems",
                _settings.CurrentValue.ReStockZeroQuantityItems.Cron,
                _settings.CurrentValue.ReStockZeroQuantityItems.Enabled
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
