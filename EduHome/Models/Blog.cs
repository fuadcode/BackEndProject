using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Models
{
    public class Blog : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        [NotMapped]
        public string ShortDesc => Desc.Length > 100 ? Desc.Substring(0, 50) : Desc;
        //public ICollection<CourseBlog> CoursesBlogs { get; set; }
    }
}
