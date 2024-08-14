using EduHome.Areas.AdminArea.ViewModels.BlogVMs;
using EduHome.Data;
using EduHome.Extensions;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class BlogController : Controller
    {
        private readonly EduCompaniesDbContext _dbContext;

        public BlogController(EduCompaniesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int stage = 1)
        {
            var query = _dbContext.Blogs
                .AsNoTracking()
                .Select(m => new BlogListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Desc = m.Desc,
                    Name = m.Name,
                    Comment = m.Comment,
                    Time = m.Time,
                    DescPart = m.DescPart,
                });

            return View(await PaginationVM<BlogListVM>.CreateVM(query, stage, 2));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(s => s.Id == id);
            if (blog == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", blog.ImgUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var blog = await _dbContext.Blogs
                .AsNoTracking()
                .Select(m => new BlogListVM()
                {
                    Id = m.Id,
                    ImgUrl = m.ImgUrl,
                    Desc = m.Desc,
                    Name = m.Name,
                    Comment = m.Comment,
                    Time = m.Time,
                    DescPart = m.DescPart,
                }).FirstOrDefaultAsync(m => m.Id == id);
            return View(blog);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blogCreateVM)
        {
            if (!ModelState.IsValid) return View(blogCreateVM);
            var file = blogCreateVM.Photo;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("Photo", "Bos ola bilmez..");
                return View(blogCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun type secin..");
                return View(blogCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Olcu boyukdur..");
                return View(blogCreateVM);
            }
            Blog blog = new()
            {
                Name = blogCreateVM.Name,
                Desc = blogCreateVM.Desc,
                Comment = blogCreateVM.Comment,
                Time = blogCreateVM.Time,
                DescPart = blogCreateVM.DescPart,
                ImgUrl = await SaveFilesAsync(file)
            };
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<string> SaveFilesAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blog");
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
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(s => s.Id == id);
            if (blog is null) return NotFound();

            var viewModel = new BlogUpdateVM
            {
                ImgUrl = blog.ImgUrl,
                Name = blog.Name,
                Desc = blog.Desc,
                Comment= blog.Comment,
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateVM blogUpdateVM)
        {
            if (id == null) return BadRequest();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(s => s.Id == id);
            if (blog == null) return NotFound();

            if (ModelState.IsValid)
            {
                blogUpdateVM.ImgUrl = blog.ImgUrl;
                return View(blogUpdateVM);
            }

            var file = blogUpdateVM.Photo;
            if (file != null)
            {
                if (file.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Boş ola bilməz..");
                    blogUpdateVM.ImgUrl = blog.ImgUrl;
                    return View(blogUpdateVM);
                }
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun type secin..");
                    return View(blogUpdateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Olcu boyukdur..");
                    return View(blogUpdateVM);
                }

                try
                {
                    string fileName = await SaveFilesAsync(file);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", blog.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    blog.ImgUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Photo", $"An error occurred while saving the file: {ex.Message}");
                    return View(blogUpdateVM);
                }
            }

            blog.Name = blogUpdateVM.Name;
            blog.Desc = blogUpdateVM.Desc;
            blog.Comment = blogUpdateVM.Comment;
            blog.Time = blogUpdateVM.Time;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}