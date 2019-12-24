using Microservice.Api.Database.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Api.Database
{
    public class MicroserviceDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public MicroserviceDbContext(DbContextOptions<MicroserviceDbContext> options)
            : base(options)
        {
        }
    }
}
