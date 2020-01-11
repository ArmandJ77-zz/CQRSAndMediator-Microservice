using Microservice.Db;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Api.Integration.Tests.Infrastructure
{
    public static class MicroserviceApiDbContextExtensions
    {
        public static void Clear(this MicroserviceDbContext db)
        {
            db.Orders.Clear();
            db.SaveChanges();
        }

        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
