using System.Threading.Tasks;
using Hangfire;

namespace Microservice.HangfireBackgroundJobServer
{
    public  interface IRecurringJob
    {
        Task Run(IJobCancellationToken cancellationToken);
    }
}
