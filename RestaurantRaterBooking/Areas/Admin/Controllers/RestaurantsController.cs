using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantRaterBooking.Models;
using X.PagedList;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestaurantsController : Controller
    {
        private readonly Models.AppContext _context;
		private readonly IWebHostEnvironment _environment;

		public RestaurantsController(Models.AppContext context, IWebHostEnvironment environment)
        {
            _context = context;
			_environment = environment;
		}

		// GET: Admin/Restaurants
		public async Task<IActionResult> Index(string searchText, int? page)
		{
			int pageSize = 5;
			int pageNumber = page ?? 1;

			IEnumerable<Restaurant> appContext = _context.Restaurant.Include(r => r.Category).Include(r => r.City);

			if (!string.IsNullOrEmpty(searchText))
			{
				// Lọc dữ liệu dựa trên searchText
				appContext = appContext.Where(x => x.Name.Contains(searchText));
			}

			var pagedListRestaurants = await appContext.ToPagedListAsync(pageNumber, pageSize);
			return View(pagedListRestaurants);
		}

		// GET: Admin/Restaurants/Create
		public IActionResult Create()
        {
			ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name");
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name");
			ViewData["Tags"] = _context.Tag.Where(t => t.TagType == TagType.Restaurant).ToList();

			return View();
		}

        // POST: Admin/Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Restaurant restaurant, [FromForm] List<IFormFile> restaurantImages, [FromForm] List<IFormFile> menuImages, List<Guid> selectedTags)
		{
			if (ModelState.IsValid)
			{
				restaurant.Id = Guid.NewGuid();
				restaurant.Images = new List<Image>();

				// Xử lý lưu hình ảnh nhà hàng
				foreach (var file in restaurantImages)
				{
					var imagePath = await SaveFile(file, "resImages");
					var image = new Image
					{
						Id = Guid.NewGuid(),
						ImagePath = imagePath,
						ImageType = ImageType.RestaurantImage,
						RestaurantID = restaurant.Id
					};

					restaurant.Images.Add(image);
				}
				// Xử lý lưu hình ảnh menu
				foreach (var file in menuImages)
				{
					var imagePath = await SaveFile(file, "menuImages");
					var image = new Image
					{
						Id = Guid.NewGuid(),
						ImagePath = imagePath,
						ImageType = ImageType.MenuImage,
						RestaurantID = restaurant.Id
					};

					restaurant.Images.Add(image);
				}

				// Thêm các thẻ tag vào cơ sở dữ liệu
				foreach (var tagId in selectedTags)
				{
					var restaurantTag = new RestaurantTag
					{
						RestaurantId = restaurant.Id,
						TagId = tagId
					};

					_context.Add(restaurantTag);
				}

				_context.Add(restaurant);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["Tags"] = _context.Tag.ToList();
			ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", restaurant.CategoryID);
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", restaurant.CityID);
			return View(restaurant);
		}

		private async Task<string> SaveFile(IFormFile file, string subFolder)
		{
			// Tạo tên file mới bằng cách thêm một giá trị Guid phía trước
			var newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

			// Tạo đường dẫn tương đối
			var relativePath = Path.Combine("Uploads", "Restaurants", subFolder, newFileName);

			// Tạo đường dẫn tuyệt đối để lưu file vào thư mục
			var absolutePath = Path.Combine(
				Directory.GetCurrentDirectory(), "wwwroot", relativePath);

			using (var stream = new FileStream(absolutePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return relativePath; // Trả về đường dẫn tương đối
		}



		// GET: Admin/Restaurants/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
        {
			//if (id == null || _context.Restaurant == null)
			//{
			//    return NotFound();
			//}

			//         var restaurant = await _context.Restaurant.FindAsync(id);
			//         if (restaurant == null)
			//         {
			//             return NotFound();
			//         }

			//ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", restaurant.CategoryID);
			//ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name", restaurant.CityID);
			//ViewData["Tags"] = _context.Tag.ToList();
			//return View(restaurant);

			// Lấy thông tin nhà hàng từ database
			var restaurant = await _context.Restaurant
										   .Include(r => r.RestaurantTags)
										   .Include(r => r.Images)
										   .FirstOrDefaultAsync(r => r.Id == id);

			if (restaurant == null)
			{
				return NotFound();
			}

			// Lấy danh sách hình ảnh nhà hàng
			var restaurantImages = restaurant.Images
											 .Where(img => img.ImageType == ImageType.RestaurantImage)
											 .ToList();

			// Lấy danh sách hình ảnh menu
			var menuImages = restaurant.Images
									   .Where(img => img.ImageType == ImageType.MenuImage)
									   .ToList();

			// Chuyển đổi danh sách hình ảnh thành danh sách URL hình ảnh
			var restaurantImageUrls = restaurantImages.Select(img => img.ImagePath).ToList();
			var menuImageUrls = menuImages.Select(img => img.ImagePath).ToList();

			// Lấy danh sách ID của tags
			var currentTags = restaurant.RestaurantTags.Select(rt => rt.TagId).ToList();

			// Gửi dữ liệu đến view thông qua ViewData
			ViewData["CurrentRestaurantImages"] = restaurantImageUrls;
			ViewData["CurrentMenuImages"] = menuImageUrls;
			ViewData["CurrentTags"] = currentTags;
			ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", restaurant.CategoryID);
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name", restaurant.CityID);
			ViewData["Tags"] = _context.Tag.ToList();

			// Gửi model đến view
			return View(restaurant);
		}

        // POST: Admin/Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", restaurant.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", restaurant.CityID);
            return View(restaurant);
        }

		
        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.Restaurant == null)
			{
				return Problem("Entity set 'AppContext.Restaurant'  is null.");
			}

			var restaurant = await _context.Restaurant
				.Include(r => r.Images)
				.Include(r => r.RestaurantTags)
				.SingleOrDefaultAsync(r => r.Id == id);

			if (restaurant != null)
			{
				foreach (var image in restaurant.Images)
				{
					string fullPath = Path.Combine(_environment.WebRootPath, image.ImagePath);
					if (System.IO.File.Exists(fullPath))
					{
						System.IO.File.Delete(fullPath);
					}
				}


				// Xóa các ảnh trong bảng Image
				_context.Image.RemoveRange(restaurant.Images);

				// Xóa các tag trong bảng RestaurantTag
				_context.RestaurantTag.RemoveRange(restaurant.RestaurantTags);

				// Xóa bản ghi Restaurant
				_context.Restaurant.Remove(restaurant);

				// Lưu thay đổi
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

        private bool RestaurantExists(Guid id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }

		[HttpGet]
		public JsonResult GetTags(string searchTerm)
		{
			// Tìm các tags dựa trên từ khóa
			var tags = _context.Tag
				.Where(t => t.Name.Contains(searchTerm))
				.Select(t => new { id = t.Id, name = t.Name })
				.ToList();

			return Json(tags);
		}
	}
}
