using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseListVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        [Required, StringLength(100)]
        public string Name{ get; set; }
        [StringLength(200)]
        public string Desc { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
