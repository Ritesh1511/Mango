using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder); //specifically for indentity insert.


            modelbuilder.Entity<Coupon>().HasData(

                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "10OAB",
                    DiscountAmount = 100,
                    MinAmount = 200
                },
                new Coupon
                {
                    CouponId = 2,
                    CouponCode = "20AB",
                    DiscountAmount = 400,
                    MinAmount = 500
                });
                
        }
    }
}
