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
		public async Task<IActionResult> Index(int? page)
		{
			int pageSize = 8;
			int pageNumber = (page ?? 1);

			IEnumerable<Restaurant> restaurants = _context.Restaurant
				.Include(r => r.Category)
				.Include(r => r.City)
				.Include(r => r.Images)
				.Include(r => r.Reviews).ToList();

			ViewData["RestaurantImages"] = restaurants.ToDictionary(
				r => r.Id,
				r => r.Images != null ? r.Images.FirstOrDefault(i => i.ImageType == ImageType.RestaurantImage)?.ImagePath : "Uploads\\NoImage.jpg"
			);

			ViewData["Restaurants"] = restaurants.ToPagedList(pageNumber, pageSize);

			ViewData["Blogs"] = _context.Blog.Include(b => b.PostCategory)
								.OrderByDescending(b => b.CreatedAt)
								.Where(b => b.IsPublish == true)
								.Take(2)
								.ToList();
			ViewData["News"] = _context.News.Include(n => n.PostCategory)
								.OrderByDescending(n => n.CreatedAt)
								.Where(n => n.IsPublish == true)
								.Take(2)
								.ToList();
			ViewData["Categories"] = _context.Category.ToList();
			ViewData["Cites"] = _context.City.ToList();

			ViewData["Sliders"] = _context.Slider.ToList();

			ViewData["NumberOfAccounts"] = _context.Users.Count();
			ViewData["NumberOfRestaurants"] = _context.Restaurant.Count();
			ViewData["NumberOfReviews"] = _context.Review.Count();
			ViewData["NumberOfBookings"] = _context.Booking.Count();

			foreach (var restaurant in restaurants)
			{
				double averageRating = restaurant.Reviews.Any() ? restaurant.Reviews.Average(r => r.Rating) : 0;

				// Làm tròn đến 1 chữ số sau dấu phẩy
				restaurant.AverageRating = Math.Round(averageRating, 1);
			}

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

		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}
	}
}