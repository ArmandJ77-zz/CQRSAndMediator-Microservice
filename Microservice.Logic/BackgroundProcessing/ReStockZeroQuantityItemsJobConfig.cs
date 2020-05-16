using Microservice.HangfireBackgroundJobServer.Extensions;

namespace Microservice.Logic.BackgroundProcessing
{
    public class ReStockZeroQuantityItemsJobConfig : RecurringJobConfig
    {
        public int ZeroQuantityLimit { get; set; }
    }
}