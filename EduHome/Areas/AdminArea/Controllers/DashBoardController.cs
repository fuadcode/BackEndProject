using Microsoft.AspNetCore.Mvc;
using EduHome.Data;


namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DashBoardController : Controller
    {
        private readonly EduCompaniesDbContext _context;

        public DashBoardController(EduCompaniesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
          
            return View();
        }
    }
}
