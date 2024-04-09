using AllUpMVC.Models;
using AllupP238.Models;
using AllupWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AllupP238.Data
{
    public class AllupDbContext: DbContext
    {
        internal object Users;

        public AllupDbContext(DbContextOptions<AllupDbContext> options): base(options)
        {
            
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
