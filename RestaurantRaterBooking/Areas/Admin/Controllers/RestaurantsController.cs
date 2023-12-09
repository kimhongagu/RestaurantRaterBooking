using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
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

		public async Task<IActionResult> Edit(Guid? id)
		{
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
			ViewData["Tags"] = _context.Tag.Where(t => t.TagType == TagType.Restaurant).ToList();

			// Gửi model đến view
			return View(restaurant);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [FromForm] Restaurant restaurant, [FromForm] List<IFormFile> restaurantImages, [FromForm] List<IFormFile> menuImages, List<Guid> selectedTags)
		{
			if (id != restaurant.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Cập nhật hình ảnh
					await UpdateImagesAsync(restaurant, restaurantImages, "resImages", ImageType.RestaurantImage);
					await UpdateImagesAsync(restaurant, menuImages, "menuImages", ImageType.MenuImage);

					// Cập nhật tags
					await UpdateTagsAsync(restaurant, selectedTags);

					_context.Update(restaurant);
					_context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}

				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}
			ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", restaurant.CategoryID);
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", restaurant.CityID);
			return View(restaurant);
		}

		private async Task UpdateImagesAsync(Restaurant restaurant, List<IFormFile> newImages, string subFolder, ImageType imageType)
		{
			if (newImages.Count() != 0)
			{
				var imagesToDelete = _context.Image
					.Where(i => i.ImageType == imageType && i.RestaurantID == restaurant.Id)
					.ToList();

				foreach (var existingImage in imagesToDelete)
				{
					DeleteFile(existingImage.ImagePath);
					_context.Image.Remove(existingImage);
				}

				// Lưu và liên kết các hình ảnh mới
				foreach (var newImage in newImages)
				{
					var imagePath = await SaveFile(newImage, subFolder);

					var image = new Image
					{
						ImagePath = imagePath,
						ImageType = imageType,
						RestaurantID = restaurant.Id
					};

					restaurant.Images.Add(image);
				}

				await _context.SaveChangesAsync();
			}
		}

		private async Task UpdateTagsAsync(Restaurant restaurant, List<Guid> selectedTags)
		{
			// Xóa hết các mối quan hệ tags của nhà hàng đó
			var existingTags = await _context.RestaurantTag
				.Where(rt => rt.RestaurantId == restaurant.Id)
				.ToListAsync();

			_context.RestaurantTag.RemoveRange(existingTags);

			// Thêm mới những mối quan hệ cho các tags được chọn
			foreach (var tagId in selectedTags)
			{
				var restaurantTag = new RestaurantTag
				{
					RestaurantId = restaurant.Id,
					TagId = tagId
				};

				_context.Update(restaurantTag);
			}

			await _context.SaveChangesAsync();
		}

		private async Task<string> SaveFile(IFormFile file, string subFolder)
		{
			var newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			var relativePath = Path.Combine("Uploads", "Restaurants", subFolder, newFileName);
			var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

			var destinationFolder = Path.GetDirectoryName(absolutePath);
			if (!Directory.Exists(destinationFolder))
			{
				Directory.CreateDirectory(destinationFolder);
			}

			using (var stream = new FileStream(absolutePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return relativePath;
		}

		private void DeleteFile(string relativePath)
		{
			var absolutePath = Path.Combine(
				Directory.GetCurrentDirectory(), "wwwroot", relativePath);

			if (System.IO.File.Exists(absolutePath))
			{
				System.IO.File.Delete(absolutePath);
			}
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

				_context.Image.RemoveRange(restaurant.Images);

				_context.RestaurantTag.RemoveRange(restaurant.RestaurantTags);

				_context.Restaurant.Remove(restaurant);

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
