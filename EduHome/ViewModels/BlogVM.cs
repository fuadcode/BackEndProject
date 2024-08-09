using EduHome.Models;

namespace EduHome.ViewModels
{
    public class BlogVM
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Comment { get; set; }
        public string ImgURl { get; set; }
        public string DescPart { get; set; }
        public DateTime Time { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
