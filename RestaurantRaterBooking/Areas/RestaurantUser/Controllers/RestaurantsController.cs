using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.RestaurantUser.Controllers
{
    [Area("RestaurantUser")]
    public class RestaurantsController : Controller
    {
        private readonly Models.AppContext _context;

        public RestaurantsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: RestaurantUser/Restaurants
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Restaurant.Include(r => r.Category).Include(r => r.City);
            return View(await appContext.ToListAsync());
        }

        // GET: RestaurantUser/Restaurants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Category)
                .Include(r => r.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: RestaurantUser/Restaurants/Edit/5
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

        // POST: RestaurantUser/Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    await UpdateImagesAsync(restaurant, restaurantImages, "resImages", ImageType.RestaurantImage);

                    await UpdateImagesAsync(restaurant, menuImages, "menuImages", ImageType.MenuImage);

                    // Cập nhật tags
                    var existingTags = _context.RestaurantTag
	                    .Where(rt => rt.RestaurantId == restaurant.Id && !selectedTags.Contains(rt.TagId))
	                    .ToList();

					foreach (var existingTag in existingTags)
					{
						_context.RestaurantTag.Remove(existingTag);
					}

					// Thêm mới những mối quan hệ
					foreach (var tagId in selectedTags)
					{
						var restaurantTag = new RestaurantTag
						{
							RestaurantId = restaurant.Id,
							TagId = tagId
						};

						_context.Update(restaurantTag);
					}

					_context.Update(restaurant);
					_context.SaveChangesAsync();
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
				return RedirectToAction("Index", "Home", new { area = "RestaurantUser" });
			}
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", restaurant.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", restaurant.CityID);
			return View(restaurant);
		}

		private async Task UpdateImagesAsync(Restaurant restaurant, List<IFormFile> newImages, string subFolder, ImageType imageType)
		{
			var restaurantTask = _context.Restaurant
	.Include(r => r.Images)
	.FirstOrDefaultAsync(r => r.Id == restaurant.Id);

            restaurant = restaurantTask.Result;
			// Kiểm tra xem restaurant.Images có tồn tại không
			if (restaurant.Images != null)
			{
				// Xóa tất cả hình ảnh của loại đã chọn
				foreach (var existingImage in restaurant.Images.Where(i => i.ImageType == imageType).ToList())
				{
					_context.Image.Remove(existingImage);
					DeleteFile(existingImage.ImagePath); // Hàm xóa file từ hệ thống tệp
				}
			}

			// Lưu và liên kết các hình ảnh mới
			foreach (var newImage in newImages)
			{
				var imagePath = await SaveFile(newImage, subFolder);

				var image = new Image
				{
					Id = Guid.NewGuid(),
					ImagePath = imagePath,
					ImageType = imageType,
					RestaurantID = restaurant.Id
				};

				restaurant.Images.Add(image);
			}

			// Lưu thay đổi vào cơ sở dữ liệu
			_context.SaveChangesAsync();
		}

		private async Task<string> SaveFile(IFormFile file, string subFolder)
		{
			// Tạo tên file mới bằng cách thêm một giá trị Guid phía trước
			var newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

			// Tạo đường dẫn tương đối
			var relativePath = Path.Combine("Uploads", "Restaurants", subFolder, newFileName);

			// Tạo đường dẫn tuyệt đối để lưu file vào thư mục
			var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

			// Tạo thư mục đích nếu nó không tồn tại
			var destinationFolder = Path.GetDirectoryName(absolutePath);
			if (!Directory.Exists(destinationFolder))
			{
				Directory.CreateDirectory(destinationFolder);
			}

			using (var stream = new FileStream(absolutePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return relativePath; // Trả về đường dẫn tương đối
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

		[HttpGet]
		public async Task<IActionResult> ViewReviews(Guid restaurantId)
		{
            ViewData["RestaurantId"] = restaurantId;
            // Lấy danh sách đánh giá cho nhà hàng
            var reviews = await _context.Review
                .Include(r => r.User)
                .Where(r => r.RestaurantID == restaurantId)
                .ToListAsync();

            // Thêm danh sách đánh giá vào ViewData
            ViewData["Reviews"] = reviews;

            var replyDictionary = new Dictionary<Guid, List<Reply>>();

            foreach (var review in reviews)
            {
                var replies = await _context.Reply
                    .Where(r => r.ReviewID == review.Id)
                    .ToListAsync();

                replyDictionary.Add(review.Id, replies);
            }

            ViewData["Replies"] = replyDictionary;

            return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddReply(Guid reviewId, string replyComment, Guid restaurantId)
		{
			try
			{
				// Kiểm tra xem review có tồn tại không
				var existingReview = await _context.Review.FindAsync(reviewId);
				if (existingReview == null)
				{
					return NotFound("Review not found");
				}

				// Tạo một đối tượng Reply từ dữ liệu nhận được
				var reply = new Reply
				{
					ReplyComment = replyComment,
				};

				// Gán ReviewID cho reply
				reply.ReviewID = reviewId;
                reply.ReplyAt = DateTime.Now;
				// Thêm reply vào cơ sở dữ liệu
				_context.Reply.Add(reply);
				await _context.SaveChangesAsync();

                return RedirectToAction("ViewReviews", "Restaurants", new { area = "RestaurantUser", restaurantId = restaurantId });
            }
			catch (Exception ex)
			{
				// Xử lý lỗi ở đây
				return StatusCode(500, "Internal server error");
			}
		}

		private bool RestaurantExists(Guid id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
