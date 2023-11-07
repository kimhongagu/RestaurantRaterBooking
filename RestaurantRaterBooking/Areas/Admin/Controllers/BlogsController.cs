using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly Models.AppContext _context;
        private readonly IWebHostEnvironment _environment;

        public BlogsController(Models.AppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Blog.Include(b => b.PostCategory);
            return View(await appContext.ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .Include(b => b.PostCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory.Where(c => c.PostType == PostType.Blog), "Id", "Name");
            ViewData["Tags"] = _context.Tag.Where(t => t.TagType == TagType.Blog).ToList();
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Blog blog, List<Guid> selectedTags)
        {
            if (ModelState.IsValid)
            {
                blog.Id = Guid.NewGuid();

                if (blog.CoverImage != null && blog.CoverImage.Length > 0)
                {
                    // Kiểm tra dung lượng tệp tải lên
                    if (blog.CoverImage.Length <= 10 * 1024 * 1024) // 10MB
                    {
                        string folder = "Uploads/Blogs";
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(blog.CoverImage.FileName);
                        string filePath = Path.Combine(_environment.WebRootPath, folder, uniqueFileName);

                        // Tạo thư mục nếu không tồn tại
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));

                        // Lưu tệp tải lên vào máy chủ
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await blog.CoverImage.CopyToAsync(stream);
                        }
                        // Cập nhật đường dẫn đến tệp tải lên
                        blog.Image = "/" + folder + "/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("CoverImage", "Dung lượng tệp tải lên quá lớn (tối đa 10MB).");
                        return View(blog);
                    }
                }

                // Thêm các thẻ tag vào cơ sở dữ liệu
                foreach (var tagId in selectedTags)
                {
                    var blogTag = new BlogTag
                    {
                        BlogId = blog.Id,
                        TagId = tagId
                    };
                    _context.Add(blogTag);
                }
                blog.CreatedAt = DateTime.Now;
                blog.EditedAt = DateTime.Now;
                blog.Alias = Models.Filter.FilterChar(blog.Title);
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", blog.PostCategoryID);
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", blog.PostCategoryID);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ShortContent,Content,Image,IsPublish,IsHot,CreatedAt,CreatedBy,EditedAt,EditedBy,PostCategoryID")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", blog.PostCategoryID);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Blog == null)
        //    {
        //        return NotFound();
        //    }

        //    var blog = await _context.Blog
        //        .Include(b => b.PostCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(blog);
        //}

        // POST: Admin/Blogs/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Blog == null)
            {
                return Problem("Entity set 'AppContext.News'  is null.");
            }
            var blog = await _context.Blog
                .Include(n => n.PostCategory)
                .Include(t => t.BlogTags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog != null)
            {
                string fullPath = Path.Combine(_environment.WebRootPath, blog.Image.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                _context.BlogTag.RemoveRange(blog.BlogTags);
                _context.Blog.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(Guid id)
        {
          return (_context.Blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
