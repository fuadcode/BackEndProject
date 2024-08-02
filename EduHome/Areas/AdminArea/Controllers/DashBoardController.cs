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

        public IActionResult Index()
        {
            var userCount = _context.Users.Count();
            var subscriptionCount = _context.Subscriptions.Count();
            var contactFormCount = _context.ContactFormModels.Count();

            ViewBag.UserCount = userCount;
            ViewBag.SubscriptionCount = subscriptionCount;
            ViewBag.ContactFormCount = contactFormCount;

            return View();
        }
    }
}

