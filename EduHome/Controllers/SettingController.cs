using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class SettingController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public SettingController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}

