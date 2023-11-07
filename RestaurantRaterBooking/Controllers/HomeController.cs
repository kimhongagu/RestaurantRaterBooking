using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using System.Diagnostics;
using X.PagedList;

namespace RestaurantRaterBooking.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly Models.AppContext _context;

        public HomeController(ILogger<HomeController> logger, Models.AppContext context)
		{
			_logger = logger;
			_context = context;
		}
        public IActionResult Index(int? page)
		{
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            IEnumerable<Restaurant> restaurants = _context.Restaurant
                .Include(r => r.Category)
                .Include(r => r.City)
                .Include(r => r.Images);

            ViewData["RestaurantImages"] = restaurants.ToDictionary(
                r => r.Id,
                r => r.Images.FirstOrDefault(i => i.ImageType == ImageType.RestaurantImage)?.ImagePath
            );

            ViewData["Restaurants"] = restaurants.ToPagedList(pageNumber, pageSize);

			ViewData["Blogs"] = _context.Blog.Include(b => b.PostCategory)
                                .OrderByDescending(b => b.CreatedAt)
                                .Take(2)
                                .ToList();
            ViewData["News"] = _context.News.Include(n => n.PostCategory)
                                .OrderByDescending(b => b.CreatedAt)
                                .Take(2)
                                .ToList();
            ViewData["Categories"] = _context.Category.ToList();

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

		public IActionResult AboutUs()
		{
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}