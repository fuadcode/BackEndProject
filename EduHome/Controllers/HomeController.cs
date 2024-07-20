using EduHome.Data;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public HomeController(EduHomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.AsNoTracking().ToList();
            var sliderText = _context.SliderText.AsNoTracking().FirstOrDefault();
            var events = _context.Event.AsNoTracking().ToList();
            var setting =_context.Settings.AsNoTracking().ToList(); 
            var main = _context.Mains.AsNoTracking().ToList();
            var card=_context.Cards.AsNoTracking().ToList();
            var cardtext =_context.CardTexts.AsNoTracking().FirstOrDefault();
           
            HomeVM homeVM = new()
            {
                Sliders = sliders,
                SliderText = sliderText,
                Event = events,
                Settings = setting,
                Mains=main,
                Cards=card,
                CardTexts=cardtext
                
               
             

            };
            return View(homeVM);
        }
    }
}
