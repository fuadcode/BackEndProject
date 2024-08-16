using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class SubscriptionViewComponent : ViewComponent
    {
        private readonly EduCompaniesDbContext _dbContext;

        public SubscriptionViewComponent(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var subscriptions = _dbContext.Subscriptions.ToList();
            return View(await Task.FromResult(subscriptions));
        }
    }
}