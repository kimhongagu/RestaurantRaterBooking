using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using X.PagedList;

namespace RestaurantRaterBooking.Controllers
{
    public class TagsController : Controller
    {
        private readonly Models.AppContext _context;

        public TagsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
              return _context.Tag != null ? 
                          View(await _context.Tag.ToListAsync()) :
                          Problem("Entity set 'AppContext.Tag'  is null.");
        }

        // GET: Tags/BlogByTag/5
        public async Task<IActionResult> BlogByTag(Guid? id, int? page)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy tag từ database
            var tag = await _context.Tag
                .Include(t => t.BlogTags)
                .ThenInclude(bt => bt.Blog)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            // Lấy danh sách blogs có chứa tag này
            var blogs = tag.BlogTags.Select(bt => bt.Blog).AsQueryable();
            
            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<Blog> pagedBlogs = blogs.ToPagedList(pageNumber, pageSize);
            
            // Lưu vào ViewData
            ViewData["Blogs"] = pagedBlogs;

            ViewData["Tag"] = tag;
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.Blog).ToListAsync();
            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.Blog).ToListAsync();
            return View("BlogByTag", pagedBlogs);
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> NewsByTag(Guid? id, int? page)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy tag từ database
            var tag = await _context.Tag
                .Include(t => t.NewsTags)
                .ThenInclude(bt => bt.News)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            // Lấy danh sách blogs có chứa tag này
            var news = tag.NewsTags.Select(bt => bt.News).AsQueryable();

            // Thiết lập phân trang
            int pageSize = 4; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1 nếu không có giá trị

            // Tạo đối tượng IPagedList
            IPagedList<News> pagedNews = news.ToPagedList(pageNumber, pageSize);

            // Lưu vào ViewData
            ViewData["News"] = pagedNews;

            ViewData["Tag"] = tag;
            ViewData["Tags"] = await _context.Tag.Where(t => t.TagType == TagType.News).ToListAsync();
            ViewData["Categories"] = await _context.PostCategory.Where(n => n.PostType == PostType.News).ToListAsync();
            return View("NewsByTag", pagedNews);
        }

        private bool TagExists(Guid id)
        {
          return (_context.Tag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
