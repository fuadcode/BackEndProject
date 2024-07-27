
using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.BlogVMs
{
    public class BlogCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
