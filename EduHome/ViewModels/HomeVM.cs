using EduHome.Models;


namespace EduHome.ViewModels
{
    public class HomeVM
    {
        public Dictionary<string, string> WebSettings { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public Subscription Subscriptions { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
