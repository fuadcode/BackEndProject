using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduCompaniesDbContext _context;

        public ContactController(EduCompaniesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var websettings = _context.WebSettings.AsNoTracking().ToDictionary(k => k.Key, k => k.Value);
            return View(websettings);
        }
    }
}
