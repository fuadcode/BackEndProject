
//using EduHome.Areas.AdminArea.ViewModels.CourseVMs;
//using EduHome.Data;
//using EduHome.Services.Interfaces;
//using EduHome.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace EduHome.Areas.AdminArea.Controllers
//{
//    [Area("AdminArea")]
//    public class CourseController : Controller
//    {
//        private readonly EduCompaniesDbContext _dbContext;
//        private readonly IEmailService _emailService;

//        public CourseController(EduCompaniesDbContext dbContext, IEmailService emailService)
//        {
//            _dbContext = dbContext;
//            _emailService = emailService;
//        }

//        public async Task<IActionResult> Index(int page = 1)
//        {
//            var query = _dbContext.Courses
//                .Include(m => m.Desc)
//                .AsNoTracking()
//                .Select(m => new CourseListVM()
//                {
//                    Name = m.Name,
//                    ImgUrl = m.ImgUrl,
//                    Desc = m.Desc,    
//                });

//            return View(await PaginationVM<CourseListVM>.CreateVM(query, page, 2));
//        }
     
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id is null) return BadRequest();
//            var course = await _dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
//            if (course is null) return NotFound();
//            _dbContext.Remove(course);
//            await _dbContext.SaveChangesAsync();
//            return RedirectToAction("Index");


//        }
//    }
//}
