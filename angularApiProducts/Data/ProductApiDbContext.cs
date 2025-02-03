using Microsoft.EntityFrameworkCore;
using angularApiProducts.Models;

namespace angularApiProducts.Data
{
    public class ProductApiDbContext : DbContext
    {
        public ProductApiDbContext(DbContextOptions<ProductApiDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Product { get; set; }
    }
}
