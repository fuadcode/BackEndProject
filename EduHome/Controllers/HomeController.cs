using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduCompaniesDbContext _context;

        public HomeController(EduCompaniesDbContext context)
        {
            _context = context;
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
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe(string email)
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
                Email = email
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
            if (ModelState.IsValid)
            {
                var messageForm = new MessageForm
                {
                    Name = model.Name,
                    Email = model.Email,
                    Subject = model.Subject,
                    Message = model.Message,
                    CreatedForm = DateTime.UtcNow
                };

                _context.MessageForms.Add(messageForm);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Message sent successfully!";
                return RedirectToAction("ContactForm");
            }

            TempData["Message"] = "Error sending message. Please try again.";
            return RedirectToAction("ContactForm");
        }
    }
}