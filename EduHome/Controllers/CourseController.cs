using EduHome.Data;
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

}