using Microsoft.AspNetCore.Identity;

namespace EduHome.Models
{
    public class AppUser : IdentityUser
    {
        public string OTPCode { get; set; }
        public string ProfilePictureUrl { get; set; } = "default-profile-picture-url";
        public ICollection<Subscription> Subscriptions { get; set; }
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        
    }
}
