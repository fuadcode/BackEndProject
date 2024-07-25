using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

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
            var teachers = _dbContext.Teachers.ToList();
            return View(await Task.FromResult(teachers));
        }

    }
}
 