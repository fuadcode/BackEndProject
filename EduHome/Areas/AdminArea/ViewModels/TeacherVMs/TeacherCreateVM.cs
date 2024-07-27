using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.TeacherVMs
{
    public class TeacherCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
