using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Batches;

namespace Microservice.HanfireWithRedisBackingStore.Infrastructure
{
    public interface IBackgroundProcessingClient
    {
        string StartNewBatch(Action<IBatchAction> action, string description);
        string Enqueue(Expression<Action> action);
        void Run(Expression<Action> action);
        Task Run(Expression<Func<Task>> action);
        void RemoveRecurringJobIfExists(string recurringJobId);

        string ContinueBatchWith(string batchId, Action<IBatchAction> action, string description = null,
            BatchContinuationOptions continuationOptions = BatchContinuationOptions.OnlyOnSucceededState);

        void ConfigureRecurringJob<T>(string recurringJobId, string cronExpression, bool enabled = true)
            where T : IRecurringJob;
    }
}
