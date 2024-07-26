using Microsoft.AspNetCore.Mvc;
using EduHome.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            var userCount = await _context.Users.CountAsync();
            var subscriptionCount = await _context.Users
                .Where(u => u.SubscriptionType != null)
                .CountAsync();

            ViewBag.UserCount = userCount;
            ViewBag.SubscriptionCount = subscriptionCount;

            return View();
        }
    }
}
