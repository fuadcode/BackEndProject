using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class MainController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public MainController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}


