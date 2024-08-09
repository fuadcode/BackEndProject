namespace EduHome.Models
{
    public class Speaker : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string ImgUrl { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
