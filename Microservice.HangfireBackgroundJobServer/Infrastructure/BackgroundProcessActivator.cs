using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.HangfireBackgroundJobServer.Infrastructure
{
    public class BackgroundProcessActivator : JobActivator
    {
        private readonly IServiceProvider _container;

        public BackgroundProcessActivator(IServiceProvider container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            return _container.CreateScope().ServiceProvider.GetRequiredService(type);
        }
    }
}
