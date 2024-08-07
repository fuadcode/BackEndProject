namespace EduHome.Models
{
    public class MessageForm : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedForm { get; set; }
        public string Status { get; set; } //Pending, Accepted, Rejected
    }
}
