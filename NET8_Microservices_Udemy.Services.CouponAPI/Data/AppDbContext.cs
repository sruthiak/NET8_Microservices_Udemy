using Microsoft.EntityFrameworkCore;
using NET8_Microservices_Udemy.Services.CouponAPI.Models;

namespace NET8_Microservices_Udemy.Services.CouponAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions ):base(dbContextOptions)
        {
            
        }
        public DbSet<Coupon> Coupons { get; set; } // table name in the DB

        //seed sample data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // required for adding Identity
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id=1,
                Code="10OFF",
                DiscountAmount=10,
                MinAmount=20
            },
            new Coupon
            {
                Id = 2,
                Code = "20OFF",
                DiscountAmount = 20,
                MinAmount = 40
            }

            );
        }
    }
}
