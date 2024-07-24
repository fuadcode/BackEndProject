using System.ComponentModel.DataAnnotations;

namespace EduHome.Models
{
    public class WebSetting : BaseEntity
    {
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
