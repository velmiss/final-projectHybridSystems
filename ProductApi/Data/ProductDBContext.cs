using Microsoft.EntityFrameworkCore;
using ProductApp.Models;

namespace ProductApi.Data
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options) : base(options)
        {
        }

        public DbSet<ProductDTO> Products { get; set; }
    }
}
