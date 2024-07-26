
using EduHome.Areas.AdminArea.ViewModels.TeacherVMs;
using EduHome.Data;
using EduHome.Extensions;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class TeacherController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public TeacherController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teacher = await _dbContext.Teachers
                .ToListAsync();
            return View(teacher);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var teachers = await _dbContext.Teachers
                .AsNoTracking()
                .Select(m => new TeacherListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Position = m.Position,
                    Name = m.Name,
                }).FirstOrDefaultAsync(m => m.Id == id);
            return View(teachers);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            if (teacher is null) return NotFound();

            var viewModel = new TeacherUpdateVM
            {
                ImgUrl = teacher.ImgUrl,
                Name = teacher.Name,
                Position = teacher.Position,
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateVM teacherUpdateVM)
        {
            if (id == null) return BadRequest();
            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            if (teacher == null) return NotFound();

            if (ModelState.IsValid)
            {
                teacherUpdateVM.ImgUrl = teacher.ImgUrl;
                return View(teacherUpdateVM);
            }

            var file = teacherUpdateVM.Photo;
            if (file != null)
            {
                if (file.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Boş ola bilməz..");
                    teacherUpdateVM.ImgUrl = teacher.ImgUrl;
                    return View(teacherUpdateVM);
                }
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun type secin..");
                    return View(teacherUpdateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu boyukdur..");
                    return View(teacherUpdateVM);
                }

                try
                {
                    string fileName = await SaveFilesAsync(file);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", teacher.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    teacher.ImgUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Photo", $"An error occurred while saving the file: {ex.Message}");
                    return View(teacherUpdateVM);
                }
            }

            teacher.Name = teacherUpdateVM.Name;
            teacher.Position = teacherUpdateVM.Position;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "teacher");
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
            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            if (teacher == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", teacher.ImgUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(TeacherCreateVM teacherCreateVM)
        {
            if (!ModelState.IsValid) return View(teacherCreateVM);
            var file = teacherCreateVM.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Bos ola bilmez..");
                return View(teacherCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun type secin..");
                return View(teacherCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Olcu boyukdur..");
                return View(teacherCreateVM);
            }
            Teacher teacher = new()
            {
                Name = teacherCreateVM.Name,
                Position = teacherCreateVM.Position,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Teachers.AddAsync(teacher);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
   