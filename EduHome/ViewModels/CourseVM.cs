using EduHome.Models;
using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class CourseVM
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Features> Features { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
