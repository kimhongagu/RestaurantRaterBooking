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
    public class CategoriesController : Controller
    {
        private readonly Models.AppContext _context;

        public CategoriesController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(Guid? id, int? page)
        {
			if (id == null || _context.Category == null)
			{
				return NotFound();
			}

			var category = await _context.Category
				.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			int pageSize = 4;
			int pageNumber = (page ?? 1);

			// Lấy danh sách nhà hàng thuộc danh mục và thực hiện phân trang
			var restaurants = _context.Restaurant
				.Where(r => r.CategoryID == id)
				.Include(r => r.Images)
				.ToPagedList(pageNumber, pageSize);

			// Truyền dữ liệu vào view
			ViewData["Category"] = category;
			ViewData["Restaurants"] = restaurants;

			var restaurantImages = new Dictionary<Guid, string>();

			foreach (var restaurant in category.Restaurants)
			{
				var restaurantImage = restaurant.Images.FirstOrDefault(i => i.ImageType == ImageType.RestaurantImage)?.ImagePath;
				if (restaurantImage != null)
				{
					restaurantImages.Add(restaurant.Id, restaurantImage);
				}
			}

			// Lưu danh sách hình ảnh vào ViewData
			ViewData["RestaurantImages"] = restaurantImages;

			return View(category);
		}

        private bool CategoryExists(Guid id)
        {
          return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
