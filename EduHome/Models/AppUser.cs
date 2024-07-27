using Microsoft.AspNetCore.Identity;

namespace EduHome.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Subscription> Subscriptions { get; set; }
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        
    }
}
