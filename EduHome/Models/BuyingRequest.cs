namespace EduHome.Models
{
    public class BuyingRequest : BaseEntity
    {
        public string CourseName { get; set; }
        public string Desc { get; set; }
        public string UserName { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        //
        public virtual Course Course { get; set; }
        public virtual AppUser User { get; set; }
    }
}
