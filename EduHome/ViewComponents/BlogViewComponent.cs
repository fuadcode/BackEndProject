using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly EduCompaniesDbContext _dbContext;

        public BlogViewComponent(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = _dbContext.Blogs.ToList();
            return View(await Task.FromResult(blogs));
        }
    }
}
