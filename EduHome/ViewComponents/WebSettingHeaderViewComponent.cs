using EduHome.Data;
using EduHome.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class WebSettingHeaderViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;
        


        public WebSettingHeaderViewComponent(EduHomeDbContext dbContext)
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

