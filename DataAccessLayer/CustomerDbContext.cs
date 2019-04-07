using Microsoft.EntityFrameworkCore;
using CustomerApi.Contracts.Models;

namespace CustomerApi.DataAccessLayer
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
                    : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
