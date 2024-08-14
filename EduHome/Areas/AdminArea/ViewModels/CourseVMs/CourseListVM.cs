using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ImgUrl { get; set; }
        public string Desc { get; set; }
        public DateTime? Date { get; set; }
    }
    }

