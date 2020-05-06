namespace Microservice.HangfireBackgroundJobServer.Extensions
{
    public class RecurringJobConfig
    {
        public bool Enabled { get; set; }
        public string Cron { get; set; }
    }
}
