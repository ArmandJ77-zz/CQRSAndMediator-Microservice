using Hangfire;
using System.Threading.Tasks;

namespace Microservice.HanfireWithRedisBackingStore
{
    public  interface IRecurringJob
    {
        Task Run(IJobCancellationToken cancellationToken);
    }
}
