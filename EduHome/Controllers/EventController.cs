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
            return View();
        }
        public IActionResult Detail(int? id)
        {
         

            EventVM eventVM = new()
            {
                
            };
            return View(eventVM);
        }

        public async Task<IActionResult> EventSearch(string text)
        {
            var data = await _dbContext.Events.Where(k => k.Name.ToLower().Contains(text.ToLower())).Take(5).ToListAsync();
            return PartialView("_EventSearchPartialView", data);
        }
    }
}

