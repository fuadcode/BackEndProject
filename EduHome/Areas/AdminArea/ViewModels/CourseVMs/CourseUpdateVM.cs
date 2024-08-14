using EduHome.Models;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseUpdateVM
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}

