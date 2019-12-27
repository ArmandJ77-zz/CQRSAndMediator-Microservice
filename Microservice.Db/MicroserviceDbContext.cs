using Microservice.Db.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Db
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
