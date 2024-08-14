using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using EduHome.Areas.AdminArea.ViewModels.EventVMs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IActionResult> Index(int stage = 1)
        {
            var query = _dbContext.Orders
                .Include(o => o.Course)
                .Select(o => new OrderVM
                {
                    OrderId = o.Id,
                    CourseName = o.Course.Name,
                    CourseDesc = o.Course.Desc,
                    CourseImageUrl = o.Course.ImgUrl,
                    OrderDate = o.OrderDate
                });

            var paginatedOrders = await PaginationVM<OrderVM>.CreateVM(query, stage, 2);

            return View(paginatedOrders);
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
