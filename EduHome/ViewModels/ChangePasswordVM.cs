using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class ChangePasswordVM
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
