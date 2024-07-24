using System.ComponentModel.DataAnnotations;

namespace EduHome.Models
{
    public class Slider : BaseEntity
    {
        public string ImgUrl { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Desc { get; set; }
    }
}
