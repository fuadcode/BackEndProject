using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class EventController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public EventController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
       
    }
}
