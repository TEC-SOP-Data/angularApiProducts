using Microsoft.EntityFrameworkCore;
using angularApiProducts.Models;

namespace angularApiProducts.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Product { get; set; }
        public DbSet<Companies> Company { get; set; }
    }
}
