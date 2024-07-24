using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class WebSettingFooterViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public WebSettingFooterViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var websetting = _dbContext.WebSettings.ToDictionary(k => k.Key, k => k.Value);
            return View(await Task.FromResult(websetting));
        }
    }
}
