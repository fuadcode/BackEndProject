namespace EduHome.Areas.AdminArea.ViewModels.SpeakerVMs
{
    public class SpeakerUpdateVM
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int EventId { get; set; }
    }
}
