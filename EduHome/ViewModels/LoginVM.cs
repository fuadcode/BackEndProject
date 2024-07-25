using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(100)]
        public string UserNameOrEmail { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
     
    }
}
