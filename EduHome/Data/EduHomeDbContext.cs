
using EduHome.Models;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Data
{
    public class EduHomeDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderText> SliderText { get; set; }

       public EduHomeDbContext() { }    
     

        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options)
       : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Your_Connection_String_Here");
            }
        }

    }
}
