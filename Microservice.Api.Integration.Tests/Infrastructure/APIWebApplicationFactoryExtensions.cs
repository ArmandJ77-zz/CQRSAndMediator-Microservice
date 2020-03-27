using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public static class APIWebApplicationFactoryExtensions
    {
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
    }
}
