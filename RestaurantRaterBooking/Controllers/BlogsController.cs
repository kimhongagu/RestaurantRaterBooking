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
    public class BlogsController : Controller
    {
        private readonly Models.AppContext _context;

        public BlogsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Blog.Include(b => b.PostCategory);
            return View(await appContext.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .Include(b => b.PostCategory)
                .Include(b => b.BlogTags)
                .ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            // Lấy danh sách các bài viết có cùng danh mục
            var relatedBlogsByCategory = await _context.Blog
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID == blog.PostCategoryID && n.Id != blog.Id)
                .Take(3)
                .ToListAsync();

            // Lấy danh sách các bài viết khác không cùng danh mục
            var relatedBlogsNotByCategory = await _context.Blog
                .Include(n => n.PostCategory)
                .Where(n => n.PostCategoryID != blog.PostCategoryID && n.Id != blog.Id)
                .Take(3)
                .ToListAsync();

            // Kết hợp hai danh sách trên
            var relatedBlogs = relatedBlogsByCategory.AsEnumerable().Concat(relatedBlogsNotByCategory.AsEnumerable()).Take(3).ToList();

            // Thêm danh sách các bài viết liên quan vào ViewData
            ViewData["RelatedBlogs"] = relatedBlogs;

            return View(blog);
        }

        private bool BlogExists(Guid id)
        {
            return (_context.Blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
