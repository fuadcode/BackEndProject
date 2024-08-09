using EduHome.Data;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public BlogController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return BadRequest();
            var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == id);
            var courses = _dbContext.Courses.AsNoTracking().ToList();
            if (blog == null && courses is null) return NotFound();

            BlogVM blogVM = new()
            {
                Name = blog.Name,
                Desc = blog.Desc,
                DescPart = blog.DescPart,
                ImgURl = blog.ImgUrl,
                Comment = blog.Comment,
                Time = blog.Time,
                Courses = courses
            };

            return View(blogVM);
        }
        public async Task<IActionResult> SearchBlog(string text)
        {
            var datas = await _dbContext.Blogs.Where(b => b.Name.ToLower().Contains(text.ToLower())).Take(5).ToListAsync();
            return PartialView("_SearchPartialView", datas);
        }
    }
}

//detail

