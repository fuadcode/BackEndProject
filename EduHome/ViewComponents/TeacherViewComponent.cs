using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly EduCompaniesDbContext _dbContext;

        public TeacherViewComponent(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teachers = await _dbContext.Teachers.AsNoTracking().ToListAsync();
            return View(await Task.FromResult(teachers));
        }

    }
}
 