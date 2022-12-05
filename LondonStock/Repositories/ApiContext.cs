using Microsoft.EntityFrameworkCore;
using LondonStock.Classes;

namespace LondonStock.Repositories
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
