using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Controllers
{
    public class NewsController : Controller
    {
        private readonly Models.AppContext _context;

        public NewsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var appContext = _context.News.Include(n => n.PostCategory);
            return View(await appContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.PostCategory)
                .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            // Lấy danh sách các bài viết có cùng danh mục
            var relatedNewsByCategory = await _context.News
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID == news.PostCategoryID && n.Id != news.Id)
                .Take(3)
                .ToListAsync();

            // Lấy danh sách các bài viết khác không cùng danh mục
            var relatedNewsNotByCategory = await _context.News
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID != news.PostCategoryID && n.Id != news.Id)
                .Take(3)
                .ToListAsync();

            // Kết hợp hai danh sách trên
            var relatedNews = relatedNewsByCategory.Concat(relatedNewsNotByCategory).Take(3).ToList();

            // Thêm danh sách các bài viết liên quan vào ViewData
            ViewData["RelatedNews"] = relatedNews;

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ShortContent,Content,Image,IsPublish,IsHot,Alias,CreatedAt,CreatedBy,EditedAt,EditedBy,PostCategoryID")] News news)
        {
            if (ModelState.IsValid)
            {
                news.Id = Guid.NewGuid();
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostCategoryID"] = new SelectList(_context.PostCategory, "Id", "Id", news.PostCategoryID);
            return View(news);
        }

        // GET: News/Edit/5
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

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ShortContent,Content,Image,IsPublish,IsHot,Alias,CreatedAt,CreatedBy,EditedAt,EditedBy,PostCategoryID")] News news)
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

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'AppContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
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
