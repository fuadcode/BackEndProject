using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EduHome.Areas.AdminArea.ViewModels.CategoryVMs
{
    public class CategoryUpdateVM
    {
        [Required, MaxLength(30)]
        [DisplayName("CategoryName")]
        public string Name { get; set; }
    }
}
