namespace EduHome.Models
{
    public class Order : BaseEntity
    {
        public int CourseId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public Course Course { get; set; }
        public AppUser User { get; set; }
    }
}
