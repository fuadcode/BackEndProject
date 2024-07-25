using EduHome.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class WebSettingFooterViewComponent : ViewComponent
    {
        private readonly EduCompaniesDbContext _dbContext;

        public WebSettingFooterViewComponent(EduCompaniesDbContext dbContext)
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
