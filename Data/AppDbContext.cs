using Microsoft.EntityFrameworkCore;
using RESTaurang.Models;

namespace RESTaurang.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Admin> Admins => Set<Admin>();
    }
}
