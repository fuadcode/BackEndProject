
using EduHome.Models;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Data
{
    public class EduHomeDbContext : DbContext
    {
        public DbSet<WebSetting> WebSettings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CourseView> CourseViews { get; set; }
        public DbSet<Teacher> Teachers { get; set; }






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
