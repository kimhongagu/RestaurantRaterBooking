using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using X.PagedList;

namespace RestaurantRaterBooking.Controllers
{
    public class PostCategoriesController : Controller
    {
        private readonly Models.AppContext _context;

        public PostCategoriesController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: PostCategories
        public async Task<IActionResult> Index()
        {
              return _context.PostCategory != null ? 
                          View(await _context.PostCategory.ToListAsync()) :
                          Problem("Entity set 'AppContext.PostCategory'  is null.");
        }

        // GET: PostCategories/BlogDetails/5
        public async Task<IActionResult> BlogDetails(Guid? id, int? page)
        {
            if (id == null || _context.PostCategory == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategory
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.Id == id && m.PostType == PostType.Blog);

            if (postCategory == null)
            {
                return NotFound();
            }

            // Lấy danh sách blogs
            var blogs = postCategory.Blog.AsQueryable();

            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<Blog> pagedBlogs = blogs.ToPagedList(pageNumber, pageSize);

            // Lưu vào ViewData
            ViewData["Blogs"] = pagedBlogs;

            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.Blog).ToListAsync();
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.Blog).ToListAsync();

            return View(postCategory);
        }

        // GET: PostCategories/NewsDetails/5
        public async Task<IActionResult> NewsDetails(Guid? id, int? page)
        {
            if (id == null || _context.PostCategory == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategory
                .Include(b => b.News)
                .FirstOrDefaultAsync(m => m.Id == id && m.PostType == PostType.News);

            if (postCategory == null)
            {
                return NotFound();
            }

            // Lấy danh sách news
            var news = postCategory.News.AsQueryable();

            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<News> pagedNews = news.ToPagedList(pageNumber, pageSize);

            // Lưu vào ViewData
            ViewData["News"] = pagedNews;

            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.News).ToListAsync();
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.News).ToListAsync();

            return View(postCategory);
        }

        public async Task<IActionResult> SearchBlog(string query, int? page)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return NotFound();
            }

            // Tìm kiếm blogs có Title hoặc ShortContent chứa chuỗi query
            var blogs = _context.Blog
                .Where(b => b.Title.Contains(query) || b.ShortContent.Contains(query))
                .AsQueryable();

            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<Blog> pagedBlogs = blogs.ToPagedList(pageNumber, pageSize);

            // Lưu vào ViewData
            ViewData["Blogs"] = pagedBlogs;

            ViewData["Query"] = query;
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.Blog).ToListAsync();
            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.Blog).ToListAsync();

            return View("SearchBlog", pagedBlogs);
        }

        public async Task<IActionResult> SearchNews(string query, int? page)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return NotFound();
            }

            // Tìm kiếm blogs có Title hoặc ShortContent chứa chuỗi query
            var news = _context.News
                .Where(b => b.Title.Contains(query) || b.ShortContent.Contains(query))
                .AsQueryable();

            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<News> pagedNews = news.ToPagedList(pageNumber, pageSize);

            // Lưu vào ViewData
            ViewData["News"] = pagedNews;

            ViewData["Query"] = query;
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.News).ToListAsync();
            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.News).ToListAsync();

            return View("SearchNews", pagedNews);
        }

        private bool PostCategoryExists(Guid id)
        {
          return (_context.PostCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
