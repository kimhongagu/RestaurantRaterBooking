using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly Models.AppContext _context;

        public RestaurantsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Restaurant.Include(r => r.Category).Include(r => r.City);
            return View(await appContext.ToListAsync());
        }

		// GET: Restaurants/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null || _context.Restaurant == null)
			{
				return NotFound();
			}

			var restaurant = await _context.Restaurant
				.Include(r => r.Category)
				.Include(r => r.Images)
				.Include(r => r.City)
				.Include(t => t.RestaurantTags)
				.ThenInclude(rt => rt.Tag)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (restaurant == null)
			{
				return NotFound();
			}

			// Lấy danh sách các nhà hàng có cùng danh mục
			var relatedRestaurantByCategory = await _context.Restaurant
				.Include(c => c.Category)
				.Include(r => r.Images)
				.Where(c => c.CategoryID == restaurant.CategoryID && c.Id != restaurant.Id)
				.Take(4)
				.ToListAsync();

			// Lấy danh sách các nhà hàng khác không cùng danh mục
			var relatedRestaurantNotByCategory = await _context.Restaurant
				.Include(n => n.Category)
				.Include(r => r.Images)
				.Where(n => n.CategoryID != restaurant.CategoryID && n.Id != restaurant.Id)
				.Take(4)
				.ToListAsync();

			// Kết hợp hai danh sách trên
			var relatedRestaurant = relatedRestaurantByCategory.Concat(relatedRestaurantNotByCategory).Take(4).ToList();

			// Thêm danh sách các nhà hàng liên quan vào ViewData
			ViewData["RelatedRestaurant"] = relatedRestaurant;

			// Lấy một hình ảnh của nhà hàng và lưu hình ảnh vào ViewData
			ViewData["RestaurantImage"] = restaurant.Images.FirstOrDefault(i => i.ImageType == ImageType.RestaurantImage)?.ImagePath;

			if (relatedRestaurant != null && relatedRestaurant.All(r => r.Images != null))
			{
				var relatedRestaurantImages = relatedRestaurant.ToDictionary(
					r => r.Id,
					r => r.Images.FirstOrDefault(i => i.ImageType == ImageType.RestaurantImage)?.ImagePath
				);
				ViewData["RelatedRestaurantImages"] = relatedRestaurantImages;
			}

			// Lấy một hình ảnh của menu nhà hàng và lưu hình ảnh vào ViewData
			ViewData["MenuImage"] = restaurant.Images.FirstOrDefault(i => i.ImageType == ImageType.MenuImage)?.ImagePath;

			// Lấy danh sách đánh giá cho nhà hàng
			var reviews = await _context.Review
				.Include(r => r.User)
				.Where(r => r.RestaurantID == id)
				.ToListAsync();

			// Thêm danh sách đánh giá vào ViewData
			ViewData["Reviews"] = reviews;

			return View(restaurant);
		}


		[HttpPost]
		public async Task<IActionResult> CreateReview(Guid restaurantId, int rating, string comment)
		{
			if (!User.Identity.IsAuthenticated)
			{
				// Nếu người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}

			// Lấy UserID từ thông tin người dùng hiện tại
			var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();

			// Tạo một đối tượng Review mới
			var review = new Review
			{
				Id = Guid.NewGuid(),
				RestaurantID = restaurantId,
				Rating = rating,
				Comment = comment,
				CreatedAt = DateTime.Now,
				EditedAt = DateTime.Now,
				UserID = userId
			};

			// Lưu vào cơ sở dữ liệu
			_context.Review.Add(review);
			await _context.SaveChangesAsync();

			// Quay lại trang chi tiết của nhà hàng hoặc trang nào đó khác
			return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
		}

		private bool RestaurantExists(Guid id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
