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
            var websettings = _dbContext.WebSettings.ToDictionary(s => s.Key, s => s.Value);
            var teacherDT = _dbContext.TeacherDetails.AsNoTracking().ToList();


            TeacherVM teacherVM = new()
            {
                WebSettings=websettings,
                TeacherDetails = teacherDT,
            };
            return View(teacherVM);
        }

    }
}