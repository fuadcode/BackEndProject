
using EduHome.Areas.AdminArea.ViewModels.CourseVMs;
using EduHome.Data;
using EduHome.Extensions;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class CourseController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public CourseController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int stage= 1)
        {
            var query = _dbContext.Courses
               .AsNoTracking()
               .Select(m => new CourseListVM()
               {
                   Id = m.Id,
                   ImgUrl = m.ImgUrl,
                   Desc = m.Desc,
                   Name = m.Name,
               });

            return View(await PaginationVM<CourseListVM>.CreateVM(query, stage, 2));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM courseCreateVM)
        {
            if (!ModelState.IsValid) return View(courseCreateVM);
            var file = courseCreateVM.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Bos ola bilmez..");
                return View(courseCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun type secin..");
                return View(courseCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Olcu boyukdur..");
                return View(courseCreateVM);
            }
            Course course = new()
            {
                Name = courseCreateVM.Name,
                Desc = courseCreateVM.Desc,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "course");
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
            var course = await _dbContext.Courses.FirstOrDefaultAsync(s => s.Id == id);
            if (course == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", course.ImgUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            var course = await _dbContext.Courses
                .AsNoTracking()
                .Select(m => new CourseListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Desc = m.Desc,
                    Name = m.Name,
                }).FirstOrDefaultAsync(m => m.Id == id);
            return View(course);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var Course = await _dbContext.Courses.FirstOrDefaultAsync(s => s.Id == id);
            if (Course is null) return NotFound();

            var viewModel = new CourseUpdateVM
            {
                ImageUrl = Course.ImgUrl,
                Name = Course.Name,
                Description = Course.Desc
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateVM CourseUpdateVM)
        {
            if (id == null) return BadRequest();
            var Course = await _dbContext.Courses.FirstOrDefaultAsync(s => s.Id == id);
            if (Course == null) return NotFound();

            if (ModelState.IsValid)
            {
                CourseUpdateVM.ImageUrl = Course.ImgUrl;
                return View(CourseUpdateVM);
            }

            var file = CourseUpdateVM.Photo;
            if (file != null)
            {
                if (file.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Boş ola bilməz..");
                    CourseUpdateVM.ImageUrl = Course.ImgUrl;
                    return View(CourseUpdateVM);
                }
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun type secin..");
                    return View(CourseUpdateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu boyukdur..");
                    return View(CourseUpdateVM);
                }

                try
                {
                    string fileName = await SaveFilesAsync(file);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", Course.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    Course.ImgUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Photo", $"An error occurred while saving the file: {ex.Message}");
                    return View(CourseUpdateVM);
                }
            }

            Course.Name = CourseUpdateVM.Name;
            Course.Desc = CourseUpdateVM.Description;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
