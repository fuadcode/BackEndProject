using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class VerifyOTPVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
