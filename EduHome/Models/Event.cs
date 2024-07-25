namespace EduHome.Models
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public DateTime Time { get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
      
    }
}
