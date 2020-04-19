using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Batches;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.HanfireWithRedisBackingStore.Infrastructure
{
    public  class BackgroundProcessingClient : IBackgroundProcessingClient
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundProcessingClient(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public string StartNewBatch(Action<IBatchAction> action, string description)
            => BatchJob.StartNew(action, description);

        public string ContinueBatchWith(
            string batchId,
            Action<IBatchAction> action,
            string description = null,
            BatchContinuationOptions continuationOptions = BatchContinuationOptions.OnlyOnSucceededState)
            => BatchJob.ContinueBatchWith(batchId, action, description, continuationOptions);

        public string Enqueue(Expression<Action> action)
            => BackgroundJob.Enqueue(action);


        public void Run(Expression<Action> action)
        {
            try
            {
                action.Compile().Invoke();
            }
            catch
            {
                BackgroundJob.Enqueue(action);
            }
        }

        public async Task Run(Expression<Func<Task>> action)
        {
            try
            {
                await action.Compile().Invoke();
            }
            catch
            {
                BackgroundJob.Enqueue(action);
            }
        }

        public void RemoveRecurringJobIfExists(string recurringJobId)
            => RecurringJob.RemoveIfExists(recurringJobId);

        public void ConfigureRecurringJob<T>(
            string recurringJobId, 
            string cronExpression, 
            bool enabled = true)
            where T : IRecurringJob
        {
            var jobRunner = _serviceProvider.CreateScope().ServiceProvider.GetService<T>();

            if (jobRunner == null)
                throw new Exception("Could not activate recurring job. Ensure it is configured on the service provider");

            if (enabled)
                RecurringJob.AddOrUpdate(
                    recurringJobId,
                    () => jobRunner.Run(JobCancellationToken.Null),
                    cronExpression
                );
            else
                RemoveRecurringJobIfExists(recurringJobId);
        }
    }
}
