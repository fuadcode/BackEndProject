
using EduHome.Areas.AdminArea.ViewModels.EventVMs;
using EduHome.Areas.AdminArea.ViewModels.UserVms;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.Services;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IOtpService otpService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _otpService = otpService;
        }


        public async Task<IActionResult> Index(string searchText, int stage = 1)
        {
            var query = string.IsNullOrEmpty(searchText)
                ? _userManager.Users
                : _userManager.Users.Where(u => u.UserName.ToLower().Contains(searchText.ToLower()) ||
                                                 u.FullName.ToLower().Contains(searchText.ToLower()));

            var pagination = await PaginationVM<AppUser>.CreateVM(query, stage, 3); 
            return View(pagination);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM createUserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserVM);
            }

            var otpCode = _otpService.GenerateOTP();
            AppUser user = new()
            {
                FullName = createUserVM.FullName,
                UserName = createUserVM.UserName,
                Email = createUserVM.Email,
                OTPCode = otpCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, createUserVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(createUserVM);
            }

            await _userManager.AddToRoleAsync(user, nameof(RolesEnum.Member));

            string body;
            using (StreamReader reader = new StreamReader("wwwroot/templates/emailTemplate/emailConfirm.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{otpCode}}", otpCode);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(new() { user.Email }, body, "OTP Verification", "Verify OTP");

            var enterOtpVM = new VerifyOTPVM { Email = user.Email, Token = otpCode };
            return View("EnterOtp", enterOtpVM);
        }

        // POST: /Admin/VerifyOtp
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string email, string otpCode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.OTPCode != otpCode)
            {
                ModelState.AddModelError("", "Invalid OTP Code.");
                return View("EnterOtp", new VerifyOTPVM { Email = email });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("EnterOtp", new VerifyOTPVM { Email = email });
            }
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "User");
        }
        public IActionResult EnterOtp(string userId)
        {
            ViewBag.UserId = userId;
            return View();
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
                    return RedirectToAction("Index"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

//             public async Task<IActionResult> VerifyEmail(string email, string token)
//{
//             AppUser user = await _userManager.FindByEmailAsync(email);
//             if (user is null) return BadRequest();
//             await _userManager.ConfirmEmailAsync(user, token);
//             await _signInManager.SignInAsync(user, true);
//             return RedirectToAction("index", "home");
//}
        
    }
}