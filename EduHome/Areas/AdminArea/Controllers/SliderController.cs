using EduHome.Areas.AdminArea.ViewModels.SliderVMs;
using EduHome.Data;
using EduHome.Extensions;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM sliderCreateVM)
        {
            if (!ModelState.IsValid) return View(sliderCreateVM);
            var file = sliderCreateVM.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Bos ola bilmez..");
                return View(sliderCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun type secin..");
                return View(sliderCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Olcu boyukdur..");
                return View(sliderCreateVM);
            }
            Slider slider = new()
            {
                Title = sliderCreateVM.Title,
                Desc = sliderCreateVM.Desc,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Sliders.AddAsync(slider);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImgUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Sliders.Remove(slider);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider is null) return NotFound();

            var viewModel = new SliderUpdateVM
            {
                ImageUrl = slider.ImgUrl,
                Title = slider.Title,
                Description = slider.Desc
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM sliderUpdateVM)
        {
            if (id == null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            if (!ModelState.IsValid)
            {
                sliderUpdateVM.ImageUrl = slider.ImgUrl;
                return View(sliderUpdateVM);
            }

            var file = sliderUpdateVM.Photo;
            if (file != null)
            {
                if (file.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Boş ola bilməz..");
                    sliderUpdateVM.ImageUrl = slider.ImgUrl;
                    return View(sliderUpdateVM);
                }
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun type secin..");
                    return View(sliderUpdateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu boyukdur..");
                    return View(sliderUpdateVM);
                }

                try
                {
                    string fileName = await SaveFilesAsync(file);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    slider.ImgUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Photo", $"An error occurred while saving the file: {ex.Message}");
                    return View(sliderUpdateVM);
                }
            }

            slider.Title = sliderUpdateVM.Title;
            slider.Desc = sliderUpdateVM.Description;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slider");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filePath = Path.Combine(directory, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return file.FileName;
        }
    }
}