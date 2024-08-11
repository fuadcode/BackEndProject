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
        var course = _context.Courses.FirstOrDefault(s=>s.Id==id);
        var blogs = _context.Blogs.AsNoTracking().ToList();
        var features = _context.Features.AsNoTracking().ToList();
        var categories = _context.Categories.Include(m => m.Course).AsNoTracking().ToList();


        CourseVM courseVM = new()
        {
            Course = course,
            Blogs = blogs,
            Features = features,
            Categories = categories,
        };
        return View(courseVM);
    }
    public IActionResult SearchCourses(string query)
    {
        var courses = _context.Courses
                              .Where(c => c.Name.Contains(query))
                              .ToList();

        return PartialView("_CourseSearchResults", courses);
    }

}