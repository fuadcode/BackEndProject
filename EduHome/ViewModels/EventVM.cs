using EduHome.Models;

namespace EduHome.ViewModels
{
    public class EventVM
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public DateTime Time { get; set; }

        public string Area { get; set; }
        public string Desc { get; set; }
        public ICollection<Course> Courses { get; set; }
        //public ICollection<Speaker> Speakers { get; set; }
    }
}
