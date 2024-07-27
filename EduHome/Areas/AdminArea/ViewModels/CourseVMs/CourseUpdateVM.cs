namespace EduHome.Areas.AdminArea.ViewModels.CourseVMs
{
    public class CourseUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string ImageUrl { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

    }
}
