using AllupP238.Models;
using Microsoft.EntityFrameworkCore;

namespace AllupP238.Data
{
    public class AllupDbContext: DbContext
    {
        public AllupDbContext(DbContextOptions<AllupDbContext> options): base(options)
        {
            
        }

        public DbSet<Slider> Sliders { get; set; }
    }
}
