
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models
{
    public class SliderText : BaseEntity
    {
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Desc { get; set; }
    }
}
