using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class OrderListController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public OrderListController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Course)
                .ToListAsync();

            var orderVMs = orders.Select(o => new OrderVM
            {
                OrderId = o.Id,
                CourseName = o.Course.Name,
                CourseDesc = o.Course.Desc,
                CourseImageUrl = o.Course.ImgUrl,
                OrderDate = o.OrderDate
            }).ToList();

            return View(orderVMs);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
