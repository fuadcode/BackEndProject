namespace EduHome.Models
{
    public class Basket : BaseEntity
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int BasketCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; }
        public Course Course { get; set; }
    }
}
