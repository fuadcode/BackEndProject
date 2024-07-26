using EduHome.Models;

namespace EduHome.ViewModels
{
    public class TeacherVM
    {
        public Dictionary<string, string> WebSettings { get; set; }
        public IEnumerable<TeacherDetail> TeacherDetails { get; set; }
    }
}
