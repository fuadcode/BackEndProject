namespace EduHome.Models
{
    public class Features : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesment { get; set; }
        public int? Price { get; set; }
    

    }
}
