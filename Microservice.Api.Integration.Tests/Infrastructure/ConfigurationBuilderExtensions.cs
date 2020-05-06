using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationSection GetCustomSection(string section)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var config = configBuilder.Build();

            return config.GetSection(section);
        }

        public static IConfigurationRoot GetConfigurationRoot()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);

            return  configBuilder.Build();
        }
    }
}
