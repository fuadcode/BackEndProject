

using EduHome.Models;

namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseDetailVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public DateTime? Date { get; set; }

      

    
    }
}
