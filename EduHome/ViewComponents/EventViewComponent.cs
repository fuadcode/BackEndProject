using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public EventViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = _dbContext.Event.ToList();
            return View(await Task.FromResult(events));
        }

    }
}
    
