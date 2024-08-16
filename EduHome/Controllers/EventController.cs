using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EduHome.Controllers
{
    public class EventController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public EventController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            ViewBag.EventCounts = _dbContext.Events.Count();
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var data = _dbContext.Events.Include(i => i.Speakers).FirstOrDefault(k => k.Id == id);
            var courses = _dbContext.Courses.AsNoTracking().ToList();
            if (data == null && courses is null) return NotFound();

            EventVM eventVM = new()
            {
                Name = data.Name,
                Desc = data.Desc,
                ImgUrl = data.ImgUrl,
                Time = data.Time,
                Area = data.Area,

                Courses = courses,
                Speakers = data.Speakers
            };
            return View(eventVM);
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            var events = _dbContext.Events.AsEnumerable() 
                .Where(c => string.IsNullOrEmpty(search) || c.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return PartialView("_EventSearchPartialView", events);
        }
    }
}

