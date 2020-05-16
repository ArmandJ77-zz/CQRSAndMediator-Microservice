using Microservice.HangfireBackgroundJobServer.Extensions;

namespace Microservice.Logic.BackgroundProcessing
{
    public class JobSettings
    {
        public RecurringJobConfig Heartbeat { get; set; }
        public RecurringJobConfig CapStockItemsToFixedQuantityRecurringJob { get; set; }

        public ReStockZeroQuantityItemsJobConfig ReStockZeroQuantityItems { get; set; }
    }
}
