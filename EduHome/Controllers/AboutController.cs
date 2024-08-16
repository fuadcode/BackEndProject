using EduHome.Data;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduCompaniesDbContext _context;

        public AboutController(EduCompaniesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courseviews = _context.CourseViews.AsNoTracking().ToList();
            var teachers = _context.Teachers.AsNoTracking().ToList();
            var testimonials = _context.Testimonials.AsNoTracking().ToList();
            var events = _context.Events.AsNoTracking().ToList();

            AboutVM aboutVm = new()
            {
                CourseViews = courseviews,
                Teachers=teachers,
                Testimonials = testimonials,
                Events=events,
            };
            return View(aboutVm);
        }
        }
    }
