
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly EduDbContext _dbContext;

        public CourseViewComponent(EduDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = _dbContext.Courses.ToList();
            return View(await Task.FromResult(courses));
        }
    }
}
