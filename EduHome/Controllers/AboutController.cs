using EduHome.Data;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace EduHome.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _context;

        public AboutController(EduHomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courseviews = _context.CourseViews.AsNoTracking().ToList();
            var teachers = _context.Teachers.AsNoTracking().ToList();

            AboutVM aboutVm = new()
            {
                CourseViews = courseviews,
                Teachers=teachers,
            };
            return View(aboutVm);
        }
    }
}