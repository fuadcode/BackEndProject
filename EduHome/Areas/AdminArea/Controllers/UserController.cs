
using EduHome.Areas.AdminArea.ViewModels.UserVms;
using EduHome.Models;
using EduHome.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;




        public UserController(UserManager<AppUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;

        }

        public async Task<IActionResult> Index(string searchText)
        {
            var users = string.IsNullOrEmpty(searchText) ? await _userManager.Users.ToListAsync()
                   : await _userManager.Users.Where(u => u.UserName.ToLower().Contains(searchText.ToLower()) ||
                u.FullName.ToLower().Contains(searchText.ToLower())).ToListAsync();

            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.RePassword)
                {
                    ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor.");
                    return View(model);
                }

                // Kullanıcı oluşturma işlemi...
                return RedirectToAction("Index"); // Başarı durumunda yönlendirme
            }

            return View(model); // Model hatalıysa formu yeniden göster
        }
    
    public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest();

            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new UserListVM
                {
                    FullName = m.FullName,
                    UserName = m.UserName,
                    Email = m.Email,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        public async Task<IActionResult> Delete(string id)
        {
            if (id is null) return BadRequest();
            var role = await _userManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            await _userManager.DeleteAsync(role);
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserUpdateVM
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); // Admin panel ana sayfasına yönlendirme
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        
    }
}