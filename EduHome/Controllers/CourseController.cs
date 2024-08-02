using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CourseController : Controller
{
    private readonly EduCompaniesDbContext _context;

    public CourseController(EduCompaniesDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.CourseCounts = _context.Courses.Count();

        return View();
    }

    [HttpGet("Search")]
    public IActionResult Search(string search)
    {
        var searchResults = new List<Course>();

        if (!string.IsNullOrEmpty(search))
        {
            searchResults = _context.Courses
                .Where(c => c.Name.Contains(search))
                .ToList();
        }

        return View(searchResults);
    }


    public IActionResult Detail(int? id)
    {
        if (id is null) return BadRequest();

        var course = _context.Courses.AsNoTracking().ToList();
        var blogs = _context.Blogs.AsNoTracking().ToList();
        var features = _context.Features.AsNoTracking().ToList();


        CourseVM courseVM = new()
        {

            Courses = course,
            Blogs = blogs,
            Features = features,
        };
        return View(courseVM);
    }
    public IActionResult LoadMore(int offset = 3)
    {
        var datas = _context.Courses.Skip(offset).Take(3).ToList();
        return PartialView("_CoursePartialView", datas);

    }

}