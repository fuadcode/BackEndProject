using EduHome.Areas.AdminArea.ViewModels.SliderVMs;
using EduHome.Data;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class SliderController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public SliderController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var slider = await _dbContext.Sliders
                .ToListAsync();
            return View(slider);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var slider = await _dbContext.Sliders
                .AsNoTracking()
                .Select(m => new SliderListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Desc = m.Desc,
                    Title = m.Title,
                }).FirstOrDefaultAsync(m => m.Id == id);
            return View(slider);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var slider = _dbContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return NotFound();

            return View(new SliderUpdateVM()
            {
                Title = slider.Title,
                Description = slider.Desc,
                ImageUrl = slider.ImgUrl,
            });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM request)
        {
            if (id == null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(p => p.Id == id);
            var file = request.Photo;

            if (slider is null) return NotFound();
            if (!ModelState.IsValid) return View(request);

            slider.Title = request.Title;
            slider.Desc = request.Description;
            slider.ImgUrl = await SaveFilesAsync(file);


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            var file = request.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Image cannot be empty!");
                return View(request);
            }
            Slider slider = new()
            {
                Title = request.Title,
                Desc = request.Desc,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Sliders.AddAsync(slider);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var filePath = Path.Combine("wwwroot/img/slider/", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"{file.FileName}";
        }
    }

}
