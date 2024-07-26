using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.AdminArea.ViewModels.UserVms
{
    public class UserListVM
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string FullName { get; set; }
        public string UserName{ get; set; }
        [StringLength(200)]
        public string Email { get; set; }
    }
}
