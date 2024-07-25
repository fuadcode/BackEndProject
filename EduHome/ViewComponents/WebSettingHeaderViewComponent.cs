using EduHome.Data;
using EduHome.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
    public class WebSettingHeaderViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;



        public WebSettingHeaderViewComponent(EduHomeDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
         
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName=user.FullName;
            }
            var websetting = _dbContext.WebSettings.ToDictionary(k => k.Key, k => k.Value);
            return View(await Task.FromResult(websetting));
        }
    }
}

