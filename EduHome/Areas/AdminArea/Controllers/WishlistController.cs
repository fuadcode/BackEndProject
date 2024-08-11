using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class WishlistController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public WishlistController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var wishlists = await _dbContext.Wishlists
                .Include(w => w.Course)
                .Include(w => w.User)
                .ToListAsync();

            return View(wishlists);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var wishlistItem = await _dbContext.Wishlists
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlistItem != null)
            {
                _dbContext.Wishlists.Remove(wishlistItem);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
