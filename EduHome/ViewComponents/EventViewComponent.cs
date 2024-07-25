
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private readonly EduCompaniesDbContext _dbContext;

        public EventViewComponent(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = _dbContext.Events.ToList();
            return View(await Task.FromResult(events));
        }

    }
}
