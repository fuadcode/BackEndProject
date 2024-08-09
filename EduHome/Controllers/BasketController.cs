using EduHome.Data;
using EduHome.Models;
using EduHome.Services;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EduHome.Controllers
{
    public class BasketController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(EduCompaniesDbContext dbContext, IEmailService emailService, UserManager<AppUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }


            if (id is null) return BadRequest();


            var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
            if (existCourse is null) return BadRequest();


            string basket = Request.Cookies["basket"];
            List<BasketVM> list;

            if (string.IsNullOrEmpty(basket))
            {
                list = new List<BasketVM>();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }


            var existBasketCourse = list.FirstOrDefault(m => m.Id == id);
            if (existBasketCourse is null)
            {

                list.Add(new BasketVM()
                {
                    Id = id.Value,
                    BasketCount = 1
                });
            }
            else
            {

                existBasketCourse.BasketCount++;
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            return RedirectToAction("ShowBasket", "Basket");
        }

        public async Task<IActionResult> ShowBasket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;

            if (string.IsNullOrEmpty(basket))
            {
                list = new List<BasketVM>();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var basketCourse in list)
                {
                    var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(m => m.Id == basketCourse.Id);
                    basketCourse.Name = existCourse.Name;
                    basketCourse.Desc = existCourse.Desc;
                    basketCourse.ImageUrl = existCourse.ImgUrl;
                }
            }

            return View(list);
        }

        public IActionResult DeleteCourseFromBasket(int? id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var deletedCourse = list.FirstOrDefault(m => m.Id == id);
            if (deletedCourse is not null)
            {
                list.Remove(deletedCourse);
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            }
            return RedirectToAction("ShowBasket");
        }

        public async Task<IActionResult> ProcessPayment(int? id)
        {
            if (id is null) return BadRequest();

            var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
            if (existCourse is null) return NotFound();


            var order = new Order
            {
                CourseId = existCourse.Id,
                OrderDate = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();


            //string basket = Request.Cookies["basket"];
            //if (basket != null)
            //{
            //    var list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            //    var courseToRemove = list.FirstOrDefault(c => c.Id == id);
            //    if (courseToRemove != null)
            //    {
            //        list.Remove(courseToRemove);
            //        Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            //    }
            //}

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Course)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            return View(order);
        }

        public async Task<IActionResult> AddToWishlist(int? id)
        {
            //login olub olmamagin yoxlayir
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id is null) return BadRequest();

            var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
            if (existCourse is null) return BadRequest();

            string wishlist = Request.Cookies["wishlist"];
            List<BasketVM> list;


            if (string.IsNullOrEmpty(wishlist))
            {
                list = new List<BasketVM>();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(wishlist);
            }

            var existWishlistCourse = list.FirstOrDefault(m => m.Id == id);
            if (existWishlistCourse is null)
            {

                list.Add(new BasketVM()
                {
                    Id = id.Value,
                    BasketCount = 1,
                    IsWishlist = true
                });
            }

            Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(list));
            return RedirectToAction("ShowWishlist");
        }


        public async Task<IActionResult> ShowWishlist()
        {
            string wishlist = Request.Cookies["wishlist"];
            List<BasketVM> list;

            if (string.IsNullOrEmpty(wishlist))
            {
                list = new List<BasketVM>();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(wishlist);
                foreach (var wishlistCourse in list)
                {
                    var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(m => m.Id == wishlistCourse.Id);
                    wishlistCourse.Name = existCourse.Name;
                    wishlistCourse.Desc = existCourse.Desc;
                    wishlistCourse.ImageUrl = existCourse.ImgUrl;
                }
            }

            return View(list);
        }

        public IActionResult RemoveFromWishlist(int? id)
        {
            string wishlist = Request.Cookies["wishlist"];
            List<BasketVM> list = JsonConvert.DeserializeObject<List<BasketVM>>(wishlist);
            var removedCourse = list.FirstOrDefault(m => m.Id == id);
            if (removedCourse is not null)
            {
                list.Remove(removedCourse);
                Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(list));
            }
            return RedirectToAction("ShowWishlist");
        }


        public async Task<IActionResult> OrderSuccess()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = await _dbContext.Orders
                .Where(o => o.UserId == userId)
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

        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("OrderSuccess");
        }

        public async Task<IActionResult> SendBuyingRequest(int courseId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var course = await _dbContext.Courses.FindAsync(courseId);
            if (course == null)
            {
                TempData[$"ErrorMessage_{courseId}"] = "The course you are trying to request does not exist.";
                return RedirectToAction("Index", "Course");
            }

         
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData[$"ErrorMessage_{courseId}"] = "User information not found.";
                return RedirectToAction("Index", "Course");
            }

            var buyingRequest = new BuyingRequest
            {
                CourseId = courseId,
                CourseName = course.Name,
                Email = user.Email, 
                UserId = userId,
                UserName = user.UserName, 
                Desc = course.Desc, 
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            _dbContext.BuyingRequests.Add(buyingRequest);
            await _dbContext.SaveChangesAsync();

            TempData[$"SuccessMessage_{courseId}"] = "Your buying request has been submitted successfully.";
            return RedirectToAction("Index", "Course");
        }
    }
}