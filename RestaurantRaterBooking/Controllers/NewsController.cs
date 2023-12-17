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
            var appContext = _context.News.Include(n => n.PostCategory).Where(n => n.IsPublish == true);
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
				.Where(n => n.IsPublish == true)
				.FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            // Lấy danh sách các bài viết có cùng danh mục
            var relatedNewsByCategory = await _context.News
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID == news.PostCategoryID && n.Id != news.Id && n.IsPublish == true)
                .Take(3)
                .ToListAsync();

            // Lấy danh sách các bài viết khác không cùng danh mục
            var relatedNewsNotByCategory = await _context.News
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID != news.PostCategoryID && n.Id != news.Id && n.IsPublish == true)
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

        private bool NewsExists(Guid id)
        {
          return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
