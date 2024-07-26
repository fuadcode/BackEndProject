using Microsoft.AspNetCore.Identity;

namespace EduHome.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
    }
}
