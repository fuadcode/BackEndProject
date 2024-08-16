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

    public IActionResult Detail(int? id)
    {
        if (id is null) return BadRequest();
        var course = _context.Courses.FirstOrDefault(s => s.Id == id);
        var blogs = _context.Blogs.AsNoTracking().ToList();
        var categories = _context.Categories.Include(m => m.Course).AsNoTracking().ToList();


        CourseVM courseVM = new()
        {
            Course = course,
            Blogs = blogs,
            Categories = categories,
        };
        return View(courseVM);
    }

    [HttpGet]
    public IActionResult Search(string search)
    {
        var courses = _context.Courses.AsEnumerable() 
            .Where(c => string.IsNullOrEmpty(search) || c.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();

        return PartialView("_CourseSearchPartialView", courses);
    }


}