using EduHome.Models;

namespace EduHome.ViewModels
{
    public class AboutVM
    {
        public IEnumerable<CourseView> CourseViews { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }

    }
}
