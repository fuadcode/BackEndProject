using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.SliderVMs
{
    public class SliderListVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Desc { get; set; }
    }
}
