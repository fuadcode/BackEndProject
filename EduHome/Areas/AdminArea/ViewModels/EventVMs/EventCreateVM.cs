
using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.EventVMs
{
    public class EventCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        public string Area { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
