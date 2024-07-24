using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public HomeController(EduHomeDbContext context)
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
            if (ModelState.IsValid)
            {
                var existingSubscription = await _context.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Email == email);

                if (existingSubscription != null)
                {
                    return Json(new { success = false, message = "Bu email artiq subscribe olunmushdur!" });
                }

                var subscription = new Subscription { Email = email };
                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();
                return Json(new {message="Subscribe Ugurlu!" });
            }

            return Json(new {message = "Invalid Email Adress!" });
        }
    }
}
