using EduHome.Data;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class CourseController : Controller
{
    private readonly EduHomeDbContext _context;

    public CourseController(EduHomeDbContext context)
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
      

        CourseVM courseVM = new()
        {
          
        };
        return View(courseVM);
    }

}

   