using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.TeacherVMs
{
    public class TeacherCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string About { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        public string Hobbies { get; set; }
        [Required]
        public string Faculty { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailAdress { get; set; }
        [Required]
        public string Skype { get; set; }
        [Required]
        public int Language { get; set; }
        [Required]
        public int TeamLeader { get; set; }
        [Required]
        public int Development { get; set; }
        [Required]
        public int Design { get; set; }
        [Required]
        public int Innovation { get; set; }
        [Required]
        public int Communication { get; set; }
    }
}
