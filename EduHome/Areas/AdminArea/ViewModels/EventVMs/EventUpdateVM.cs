namespace EduHome.Areas.AdminArea.ViewModels.EventVMs
{
    public class EventUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string ImageUrl { get; set; }
        public string Name {  get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
    }
}
