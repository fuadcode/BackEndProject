using EduHome.Areas.AdminArea.ViewModels.EventVMs;
using EduHome.Data;
using EduHome.Migrations;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IActionResult> Index(int stage = 1)
        {
            var query = _dbContext.Wishlists
                .Include(w => w.Course)
                .Include(w => w.User)
                .AsQueryable(); 
            var paginatedWishlists = await PaginationVM<Wishlist>.CreateVM(query, stage, 2);

            return View(paginatedWishlists);
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
