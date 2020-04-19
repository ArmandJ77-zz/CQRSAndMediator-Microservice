using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Pro.Redis;
using Microservice.HanfireWithRedisBackingStore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microservice.HanfireWithRedisBackingStore.Configuration
{
    public static class BackgroundJobServerServiceCollectionExtensions
    {
        public static IServiceCollection AddBackgroundJobServer(this IServiceCollection services,
            IConfiguration config)
        {
            var settings = new BackgroundJobServerSettings();
            config.Bind(settings);

            services
                .Configure<BackgroundJobServerSettings>(config)
                ;

            if (!string.IsNullOrEmpty(settings.RedisConnectionString))
            {
                services
                    .AddHangfire(o => o
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseBatches(new TimeSpan(7, 0, 0, 0))
                        .UseRedisStorage(settings.RedisConnectionString, new RedisStorageOptions
                        {
                            Database = settings.RedisDatabaseNumber
                        })
                    );
            }

            services
                .AddTransient<IBackgroundProcessingClient, BackgroundProcessingClient>();

            return services;
        }

        public static IApplicationBuilder UseBackgroundJobServerDashboard(this IApplicationBuilder app)
        {
            GlobalConfiguration.Configuration.UseActivator(new BackgroundProcessActivator(app.ApplicationServices));

            var settings = app.ApplicationServices.GetService<IOptionsMonitor<BackgroundJobServerSettings>>();

            if (!string.IsNullOrEmpty(settings.CurrentValue.RedisConnectionString))
                app
                    .UseHangfireServer(new BackgroundJobServerOptions
                    {
                        WorkerCount = 5
                    })
                    .UseHangfireDashboard("/hang", new DashboardOptions
                    {
                        Authorization = new List<IDashboardAuthorizationFilter>(),
                        IgnoreAntiforgeryToken = true
                    });

            return app;
        }
    }
}
