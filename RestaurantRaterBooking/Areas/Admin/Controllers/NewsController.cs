using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly Models.AppContext _context;
        private readonly IWebHostEnvironment _environment;

        public NewsController(Models.AppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            var appContext = _context.News.Include(n => n.PostCategory);
            return View(await appContext.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.PostCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory.Where(c => c.PostType == PostType.News), "Id", "Name");
            ViewData["Tags"] = _context.Tag.Where(t => t.TagType == TagType.News).ToList();
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] News news, List<Guid> selectedTags)
        {
            if (ModelState.IsValid)
            {
                news.Id = Guid.NewGuid();

                if (news.CoverImage != null && news.CoverImage.Length > 0)
                {
                    // Kiểm tra dung lượng tệp tải lên
                    if (news.CoverImage.Length <= 10 * 1024 * 1024) // 10MB
                    {
                        string folder = "Uploads/News";
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(news.CoverImage.FileName);
                        string filePath = Path.Combine(_environment.WebRootPath, folder, uniqueFileName);

                        // Tạo thư mục nếu không tồn tại
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));

                        // Lưu tệp tải lên vào máy chủ
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await news.CoverImage.CopyToAsync(stream);
                        }
                        // Cập nhật đường dẫn đến tệp tải lên
                        news.Image = "/" + folder + "/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("CoverImage", "Dung lượng tệp tải lên quá lớn (tối đa 10MB).");
                        return View(news);
                    }
                }

                // Thêm các thẻ tag vào cơ sở dữ liệu
                foreach (var tagId in selectedTags)
                {
                    var newsTag = new NewsTag
                    {
                        NewsId = news.Id,
                        TagId = tagId
                    };

                    _context.Add(newsTag);
                }
                news.CreatedAt = DateTime.Now;
                news.EditedAt = DateTime.Now;
                news.Alias = Models.Filter.FilterChar(news.Title);
				_context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", news.PostCategoryID);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", news.PostCategoryID);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ShortContent,Content,Image,IsPublish,IsHot,CreatedAt,CreatedBy,EđitedAt,EditedBy,PostCategoryID")] News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", news.PostCategoryID);
            return View(news);
        }

        // GET: Admin/News/Delete/5
   //     public async Task<IActionResult> Delete(Guid? id)
   //     {
   //         if (id == null || _context.News == null)
   //         {
   //             return NotFound();
   //         }

   //         var news = await _context.News
   //             .Include(n => n.PostCategory)
   //             .Include(t => t.NewsTags)
   //             .FirstOrDefaultAsync(m => m.Id == id);
   //         if (news == null)
   //         {
   //             return NotFound();
   //         }
			//string fullPath = Path.Combine(_environment.WebRootPath, news.Image);
			//if (System.IO.File.Exists(fullPath))
			//{
			//	System.IO.File.Delete(fullPath);
			//}
			//_context.NewsTag.RemoveRange(news.NewsTags);
			//_context.News.Remove(news);

			//// Lưu thay đổi
			//await _context.SaveChangesAsync();
			//return RedirectToAction(nameof(Index));
		//}

        // POST: Admin/News/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'AppContext.News'  is null.");
            }
			var news = await _context.News
				.Include(n => n.PostCategory)
				.Include(t => t.NewsTags)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (news != null)
            {
				string fullPath = Path.Combine(_environment.WebRootPath, news.Image.TrimStart('/'));

				if (System.IO.File.Exists(fullPath))
				{
					System.IO.File.Delete(fullPath);
				}
				_context.NewsTag.RemoveRange(news.NewsTags);
				_context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(Guid id)
        {
          return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
