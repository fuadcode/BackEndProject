using EduHome.Models;

namespace EduHome.ViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }

    }
}
