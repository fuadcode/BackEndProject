using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.TeacherVMs
{
    public class TeacherListVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        [Required, StringLength(100)]
        public string Name{ get; set; }
        [StringLength(200)]
        public string Position { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
