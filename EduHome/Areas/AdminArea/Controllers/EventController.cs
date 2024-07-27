using EduHome.Areas.AdminArea.ViewModels.EventVMs;
using EduHome.Data;
using EduHome.Extensions;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class EventController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public EventController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int stage = 1)
        {
            var query = _dbContext.Events
               .AsNoTracking()
               .Select(m => new EventListVM()
               {
                   Id = m.Id,
                   ImgUrl = m.ImgUrl,
                   CreatedDate = m.CreatedDate,
                   Area = m.Area,
                   Desc = m.Desc,
                   Name = m.Name,
               });

            return View(await PaginationVM<EventListVM>.CreateVM(query, stage, 2));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var ev = await _dbContext.Events
                .AsNoTracking()
                .Select(m => new EventListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Desc = m.Desc,
                    Area = m.Area,
                    CreatedDate = m.CreatedDate,
                    Name = m.Name,
                }).FirstOrDefaultAsync(m => m.Id == id);
            return View(ev);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EventCreateVM eventCreateVM)
        {
            if (!ModelState.IsValid) return View(eventCreateVM);
            var file = eventCreateVM.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Bos ola bilmez..");
                return View(eventCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun type secin..");
                return View(eventCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Olcu boyukdur..");
                return View(eventCreateVM);
            }
            Event ev = new()
            {
                Name = eventCreateVM.Title,
                Desc = eventCreateVM.Desc,
                CreatedDate = eventCreateVM.CreatedDate,
                Area = eventCreateVM.Area,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Events.AddAsync(ev);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "event");
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var ev = await _dbContext.Events.FirstOrDefaultAsync(s => s.Id == id);
            if (ev == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", ev.ImgUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Events.Remove(ev);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var ev = await _dbContext.Events.FirstOrDefaultAsync(s => s.Id == id);
            if (ev is null) return NotFound();

            var viewModel = new EventUpdateVM
            {
                ImageUrl = ev.ImgUrl,
                Name = ev.Name,
                Desc = ev.Desc,
                Area = ev.Area,
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateVM eventUpdateVM)
        {
            if (id == null) return BadRequest();
            var ev = await _dbContext.Events.FirstOrDefaultAsync(s => s.Id == id);
            if (ev == null) return NotFound();

            if (ModelState.IsValid)
            {

                eventUpdateVM.ImageUrl = ev.ImgUrl;
                return View(eventUpdateVM);
            }

            var file = eventUpdateVM.Photo;
            if (file != null)
            {
                if (file.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Boş ola bilməz..");
                    eventUpdateVM.ImageUrl = ev.ImgUrl;
                    return View(eventUpdateVM);
                }
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun type secin..");
                    return View(eventUpdateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu boyukdur..");
                    return View(eventUpdateVM);
                }

                try
                {
                    string fileName = await SaveFilesAsync(file);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", ev.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    ev.ImgUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Photo", $"An error occurred while saving the file: {ex.Message}");
                    return View(eventUpdateVM);
                }
            }
         
            ev.Name = eventUpdateVM.Name;
            ev.Desc = eventUpdateVM.Desc;
            ev.Area = eventUpdateVM.Area;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
