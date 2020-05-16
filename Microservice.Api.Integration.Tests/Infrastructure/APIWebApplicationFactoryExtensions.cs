using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public static class APIWebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<TStartup> WithAppSettings<TStartup>(
            this WebApplicationFactory<TStartup> factory,
            IEnumerable<KeyValuePair<string, string>> customSettings = null)
            where TStartup : class
            => factory.WithWebHostBuilder(x =>
            {
                var projDir = Directory.GetCurrentDirectory();
                var configpath = Path.Combine(projDir, "appsettings.json");
                x.ConfigureServices(c => c.AddSingleton(new HostingEnvironment {EnvironmentName = "Testing"}));
                x.ConfigureAppConfiguration((c, d) => d.AddJsonFile(configpath));
                if (customSettings != null)
                    x.ConfigureAppConfiguration((c, d) => d.AddInMemoryCollection(customSettings));
            });

        public static WebApplicationFactory<TStartup> Seed<TStartup, TDbContext>(
            this WebApplicationFactory<TStartup> factory,
            Action<TDbContext> seed)
            where TStartup : class
            where TDbContext : DbContext
        {
            using (IServiceScope scope = factory.Server.Services.CreateScope())
            {
                TDbContext service = scope.ServiceProvider.GetService<TDbContext>();

                seed(service);
                service.SaveChanges();
            }
            return factory;
        }

        public static TService GetScopedService<TStartup, TService>(
            this WebApplicationFactory<TStartup> factory)
            where TStartup : class
            => factory.Server.Services.CreateScope().ServiceProvider.GetService<TService>();



    }
}
