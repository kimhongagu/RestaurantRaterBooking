using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantRaterBooking.Models;
using X.PagedList;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Index(string searchText, int? page)
        {
			int pageSize = 5;
			int pageNumber = page ?? 1;

			IEnumerable<News> appContext = _context.News.Include(n => n.PostCategory);

			if (!string.IsNullOrEmpty(searchText))
			{
				appContext = appContext.Where(x => x.Title.Contains(searchText));
			}

			var pagedListNews = await appContext.ToPagedListAsync(pageNumber, pageSize);
			return View(pagedListNews);
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
                news.CreatedBy = User.Identity.Name;
                news.EditedAt = DateTime.Now;
                news.EditedBy = User.Identity.Name;
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
            var restaurant = await _context.News
                                           .Include(r => r.NewsTags)
                                           .FirstOrDefaultAsync(r => r.Id == id);
            // Lấy danh sách ID của tags
            var currentTags = restaurant.NewsTags.Select(rt => rt.TagId).ToList();
            ViewData["CurrentTags"] = currentTags;
			ViewData["PostCategoryID"] = new SelectList(_context.PostCategory.Where(c => c.PostType == PostType.News), "Id", "Name");
			ViewData["Tags"] = _context.Tag.Where(t => t.TagType == TagType.News).ToList();
            return View(news);
        }

        // POST: Admin/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] News news, List<Guid> selectedTags)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

                    var existingTags = await _context.NewsTag
                        .Where(rt => rt.NewsId == news.Id)
                        .ToListAsync();

                    _context.NewsTag.RemoveRange(existingTags);
                    foreach (var tagId in selectedTags)
                    {
                        var newsTag = new NewsTag
                        {
                            NewsId = news.Id,
                            TagId = tagId
                        };

                        _context.Update(newsTag);
                    }
                    news.EditedAt = DateTime.Now;
                    news.EditedBy = User.Identity.Name;
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
