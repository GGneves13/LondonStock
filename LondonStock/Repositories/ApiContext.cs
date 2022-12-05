using Microsoft.EntityFrameworkCore;
using teste.Classes;

namespace teste.Repositories
{
    public class ApiContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApiContext(DbContextOptions<ApiContext> context) : base(context)
        {

        }
    }
}
