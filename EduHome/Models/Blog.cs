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
        public string DescPart { get; set; }
     
    }
}
