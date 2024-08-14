using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SubscriptionController : Controller
    {
        private readonly EduCompaniesDbContext _context;

        public SubscriptionController(EduCompaniesDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int stage = 1)
        {
            var query = _context.Subscriptions.AsQueryable();

            var paginatedSubscriptions = await PaginationVM<Subscription>.CreateVM(query, stage, 3);

            return View(paginatedSubscriptions);
        }


        [HttpPost]
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                return Json(new { success = false, message = "Invalid email address!" });
            }

            var existingSubscription = await _context.Subscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == email);

            if (existingSubscription != null)
            {
                return Json(new { success = false, message = "This email is already subscribed!" });
            }

            var subscription = new Subscription
            {
                SubscriptionName = "UserSubscription",
                Email = email,
                SubscriptionDate = DateTime.Now
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Subscription Success!" });
        }


        private bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
