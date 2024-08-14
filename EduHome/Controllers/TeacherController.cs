using EduHome.Data;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public TeacherController(EduCompaniesDbContext dbContext)
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
            var teacher = _dbContext.Teachers.FirstOrDefault(b => b.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

    }
}