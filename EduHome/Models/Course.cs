

namespace EduHome.Models
{
    public class Course : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime? Date { get; set; }

      

    }
}
