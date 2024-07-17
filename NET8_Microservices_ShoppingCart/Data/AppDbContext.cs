using Microsoft.EntityFrameworkCore;
using NET8_Microservices_ShoppingCartAPI.Models;

namespace NET8_Microservices_ShoppingCartAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

    }
}
