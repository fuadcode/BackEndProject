namespace EduHome.Models
{
    public class Teacher : BaseEntity
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public string EmailAdress { get; set; }
        public int? Language { get; set; }
        public int? TeamLeader { get; set; }
        public int? Development { get; set; }
        public int? Design { get; set; }
        public int? Innovation { get; set; }
        public int? Communication { get; set; }
    }
}
