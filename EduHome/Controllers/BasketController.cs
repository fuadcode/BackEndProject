using EduHome.Data;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existBasketItem = await _dbContext.Baskets
                .FirstOrDefaultAsync(b => b.CourseId == id && b.UserId == userId);

            if (existBasketItem is null)
            {
              
                var newBasketItem = new Basket
                {
                    UserId = userId,
                    CourseId = id.Value,
                    BasketCount = 1,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Baskets.Add(newBasketItem);
            }
            else
            {
           
                existBasketItem.BasketCount++;
                _dbContext.Baskets.Update(existBasketItem);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ShowBasket", "Basket");
        }

        public async Task<IActionResult> ShowBasket()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var basketItems = await _dbContext.Baskets
                .Where(b => b.UserId == userId)
                .Include(b => b.Course) 
                .Select(b => new BasketVM
                {
                    Id = b.CourseId,
                    Name = b.Course.Name,
                    Desc = b.Course.Desc,
                    ImageUrl = b.Course.ImgUrl,
                    BasketCount = b.BasketCount
                })
                .ToListAsync();

            return View(basketItems);
        }


        public async Task<IActionResult> DeleteCourseFromBasket(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id is null) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var basketItem = await _dbContext.Baskets
                .FirstOrDefaultAsync(b => b.CourseId == id && b.UserId == userId);

            if (basketItem is not null)
            { 
                _dbContext.Baskets.Remove(basketItem);
                await _dbContext.SaveChangesAsync();
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

            var userEmail = order.User.Email;

            _emailService.SendEmail(new List<string> { userEmail },
               "Payment made successfully",
               "Payment Id : 12e30dd300d321",
               "Thank you for choosing us");

            return View(order);
        }


        public async Task<IActionResult> AddToWishlist(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id is null) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existCourse = await _dbContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
            if (existCourse is null) return BadRequest();

            var existWishlistCourse = await _dbContext.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CourseId == id);

            if (existWishlistCourse is null)
            {
                var wishlistItem = new Wishlist
                {
                    UserId = userId,
                    CourseId = id.Value,
                    ImgUrl = existCourse.ImgUrl,
                    Desc = existCourse.Desc,    
                    CreatedDate = DateTime.Now
                };
                _dbContext.Wishlists.Add(wishlistItem);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("ShowWishlist");
        }



        public async Task<IActionResult> ShowWishlist()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _dbContext.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Course)
                .ToListAsync();

            var wishlistVMs = wishlistItems.Select(w => new BasketVM
            {
                Id = w.Course.Id,
                Name = w.Course.Name,
                Desc = w.Course.Desc,
                ImageUrl = w.Course.ImgUrl
            }).ToList();

            return View(wishlistVMs);
        }



        public async Task<IActionResult> RemoveFromWishlist(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItem = await _dbContext.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CourseId == id);

            if (wishlistItem is not null)
            {
                _dbContext.Wishlists.Remove(wishlistItem);
                await _dbContext.SaveChangesAsync();
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