using Hangfire;
using MediatR;
using Microservice.HangfireBackgroundJobServer;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microservice.Logic.BackgroundProcessing
{
    public class ReStockZeroQuantityItemsRecurringJob : IRecurringJob
    {
        private readonly ILogger<ReStockZeroQuantityItemsRecurringJob> _logger;
        private readonly IMediator _mediator;
        public ReStockZeroQuantityItemsRecurringJob(ILogger<ReStockZeroQuantityItemsRecurringJob> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public Task Run(IJobCancellationToken cancellationToken)
        {
            return Task.CompletedTask; 
        }
    }
}
