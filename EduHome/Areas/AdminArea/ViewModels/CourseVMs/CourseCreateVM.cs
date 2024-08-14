
using EduHome.Models;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
