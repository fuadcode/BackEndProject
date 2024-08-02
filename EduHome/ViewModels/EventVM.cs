using EduHome.Models;


namespace EduHome.ViewModels
{
    public class EventVM
    {
        public ICollection<Event> Events { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
