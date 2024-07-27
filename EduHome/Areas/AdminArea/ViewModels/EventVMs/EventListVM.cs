using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.EventVMs
{
    public class EventListVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public DateTime? CreatedDate { get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
    }
}
