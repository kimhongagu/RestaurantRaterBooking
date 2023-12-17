using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using X.PagedList;

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
				.Include(r => r.Reviews)
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

			// Tính số sao trung bình
			double averageRating = restaurant.Reviews.Any() ? restaurant.Reviews.Average(r => r.Rating) : 0;
			restaurant.AverageRating = Math.Round(averageRating, 1);

			// Lấy một hình ảnh của menu nhà hàng và lưu hình ảnh vào ViewData
			ViewData["MenuImage"] = restaurant.Images.FirstOrDefault(i => i.ImageType == ImageType.MenuImage)?.ImagePath;

			// Lấy danh sách đánh giá cho nhà hàng
			var reviews = await _context.Review
				.Include(r => r.User)
				.Where(r => r.RestaurantID == id)
				.ToListAsync();

			// Thêm danh sách đánh giá vào ViewData
			ViewData["Reviews"] = reviews;

            //lấy danh sách các reply
            var replyDictionary = new Dictionary<Guid, List<Reply>>();

            foreach (var review in reviews)
            {
                var replies = await _context.Reply
                    .Where(r => r.ReviewID == review.Id)
                    .ToListAsync();

                replyDictionary.Add(review.Id, replies);
            }

            ViewData["Replies"] = replyDictionary;

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

		[HttpPost]
		public async Task<IActionResult> Search(Guid cityID, string? searchKeyword, int? page)
		{
			int pageSize = 4;
			int pageNumber = (page ?? 1);

			IEnumerable<Restaurant> restaurants = _context.Restaurant
				.Include(r => r.Category)
				.Include(r => r.City)
				.Include(r => r.Images)
				.Include(r => r.Reviews);

			if (cityID != Guid.Empty)
			{
				restaurants = restaurants.Where(r => r.CityID == cityID);
			}

			if (!string.IsNullOrEmpty(searchKeyword))
			{
				restaurants = restaurants.Where(r => r.Name.Contains(searchKeyword));
			}

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
			ViewData["Cites"] = _context.City.ToList();
			ViewData["Sliders"] = _context.Slider.ToList();

			foreach (var restaurant in restaurants)
			{
				double averageRating = restaurant.Reviews.Any() ? restaurant.Reviews.Average(r => r.Rating) : 0;

				// Làm tròn đến 1 chữ số sau dấu phẩy
				restaurant.AverageRating = Math.Round(averageRating, 1);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> BookTable(Guid restaurantId, int? adults, int? children, string? note, DateTime bookingDate, DateTime bookingTime, string name, string phoneNumber)
		{
			if (!User.Identity.IsAuthenticated)
			{
				// Nếu người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}
			var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
			try
			{
				if (!ModelState.IsValid)
				{
					return View();
				}

				var booking = new Booking
				{
					BookingDate = bookingDate,
					BookingTime = bookingTime,
					Adults = adults,
					Children = children,
					Note = note,
					Status = 0,
					CreatedAt = DateTime.Now,
					RestaurantID = restaurantId,
					UserID = userId,
					Name = name,
					PhoneNumber = phoneNumber
				};

				_context.Booking.Add(booking);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Đặt bàn thành công!";
				return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Đặt bàn không thành công. Vui lòng thử lại sau.";
				return View("Error");
			}
		}


		private bool RestaurantExists(Guid id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
