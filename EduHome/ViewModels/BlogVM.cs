using EduHome.Models;

namespace EduHome.ViewModels
{
    public class BlogVM
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public string ImgURl { get; set; }
    }
}
