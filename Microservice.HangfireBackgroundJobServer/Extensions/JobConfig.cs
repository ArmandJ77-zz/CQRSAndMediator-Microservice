namespace Microservice.HangfireBackgroundJobServer.Extensions
{
    public class JobConfig
    {
        public string Cron { get; set; }
        public bool Enabled { get; set; }
    }
}
