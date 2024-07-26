
using EduHome.Areas.AdminArea.ViewModels.CourseVMs;
using EduHome.Data;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CourseController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;
        private readonly IEmailService _emailService;

        public CourseController(EduCompaniesDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            return View();
        }


    }
}
     
