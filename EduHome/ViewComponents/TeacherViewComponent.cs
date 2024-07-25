using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly EduDbContext _dbContext;

        public TeacherViewComponent(EduDbContext dbContext)
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
 