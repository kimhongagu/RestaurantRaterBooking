using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

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
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Restaurant.Include(r => r.Category).Include(r => r.City);
            return View(await appContext.ToListAsync());
        }

        // GET: Admin/Restaurants/Details/5
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

        // GET: Admin/Restaurants/Create
        public IActionResult Create()
        {
			ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name");
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name");

			var restaurant = new Restaurant
			{
				Tags = new List<Tag>()
			};

			return View(restaurant);
		}

        // POST: Admin/Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Restaurant restaurant, [FromForm] List<IFormFile> restaurantImages, [FromForm] List<IFormFile> menuImages, List<string> tags)
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
				foreach (var tagName in tags)
				{
					var tag = new Tag
					{
						Id = Guid.NewGuid(),
						Name = tagName,
						TypeTag = TagType.Restaurant,
						RestaurantId = restaurant.Id
					};

					_context.Tag.Add(tag);
				}

				_context.Add(restaurant);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
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
            if (id == null || _context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", restaurant.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", restaurant.CityID);
            return View(restaurant);
        }

        // POST: Admin/Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Address,Phone,Website,Email,Offer,OpeningHour,ClosingHour,SuitableFor,SpecialFeature,Space,ParkingSpace,ChildrenChair,Wifi,AirConditioning,VisaMasterCard,VATInvoice,PrivateRoom,OutdoorTable,ChildrenPlayArea,SmokiingArea,DirectBill,Delivery,Karaoke,Projector,EnventDecoration,CityID,CategoryID")] Restaurant restaurant)
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

        // GET: Admin/Restaurants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Admin/Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Restaurant == null)
            {
                return Problem("Entity set 'AppContext.Restaurant'  is null.");
            }
            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurant.Remove(restaurant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(Guid id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
