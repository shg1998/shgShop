using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountDbContext(DbContextOptions<DiscountDbContext> options) : DbContext(options)
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(new List<Coupon>
            {
                new()
                {
                    Id = 1,
                    ProductName = "Iphone X",
                    Description = "Iphone Discount",
                    Amount = 150
                },
                new()
                {
                    Id = 2,
                    ProductName = "Samsung 10",
                    Description = "Samsung Discount",
                    Amount = 100
                }
            });
        }
    }
}
