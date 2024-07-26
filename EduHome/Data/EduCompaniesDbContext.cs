using EduHome.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Data
{
    public class EduCompaniesDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<WebSetting> WebSettings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Course> Courses { get; set; }   
        public DbSet<CourseView> CourseViews { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherDetail> TeacherDetails { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<MessageUsers> MessageUsers { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

       




        public EduCompaniesDbContext() { }


        public EduCompaniesDbContext(DbContextOptions<EduCompaniesDbContext> options)
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
