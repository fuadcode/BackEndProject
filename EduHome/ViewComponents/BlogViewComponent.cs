using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public BlogViewComponent(EduHomeDbContext dbContext)
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
