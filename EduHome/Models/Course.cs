

using EduHome.Controllers;

namespace EduHome.Models
{
    public class Course : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime? Date { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }

    }
}
