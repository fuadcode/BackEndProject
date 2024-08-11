namespace EduHome.Models
{
    public class Wishlist : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string Desc { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Course Course { get; set; }
    }
}
