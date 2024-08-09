using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduCompaniesDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(EduCompaniesDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var websettings = _context.WebSettings.ToDictionary(s => s.Key, s => s.Value);
            var sliders = _context.Sliders.AsNoTracking().ToList();
            var events = _context.Events.AsNoTracking().ToList();
            var courses = _context.Courses.AsNoTracking().ToList();
            var testimonials = _context.Testimonials.AsNoTracking().ToList();
            var blogs = _context.Blogs.AsNoTracking().ToList();
            var subscription = _context.Subscriptions.AsNoTracking().FirstOrDefault();

            HomeVM homeVM = new()
            {
                WebSettings = websettings,
                Sliders = sliders,
                Events = events,
                Courses = courses,
                Testimonials = testimonials,
                Blogs = blogs,
                Subscriptions = subscription,
            };
            return View(homeVM);
        }

        [HttpPost]
        [Route("home/subscribe")]
        public async Task<IActionResult> Subscribe(string email, string userName)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                return Json(new { success = false, message = "Invalid email address!" });
            }

            var existingSubscription = await _context.Subscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == email);

            if (existingSubscription != null)
            {
                return Json(new { success = false, message = "This email is already subscribed!" });
            }

            var subscription = new Subscription
            {
                SubscriptionName = "UserSubscription",
                Email = email,
                SubscriptionDate = DateTime.Now
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Subscription Success!" });

        }

        private bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }


        [HttpPost]
        public IActionResult SubmitContactFormw(MessageForm model)
        {
            if (ModelState.IsValid)
            {

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(MessageForm model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = "The email address is not registered. Please register to use this comment table.";
                TempData["MessageType"] = "error";
                return RedirectToAction("ContactForm");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["Message"] = "Your email address is not confirmed. Please confirm your email address before sending a message.";
                TempData["MessageType"] = "error";
                return RedirectToAction("ContactForm");
            }

            var messageForm = new MessageForm
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                CreatedForm = DateTime.UtcNow,
                Status = "Pending" 
            };

            _context.MessageForms.Add(messageForm);
            await _context.SaveChangesAsync();

            TempData["Message"] = "The message was sent successfully.";
            TempData["MessageType"] = "success";
            return RedirectToAction("ContactForm");
        }
    }
}
