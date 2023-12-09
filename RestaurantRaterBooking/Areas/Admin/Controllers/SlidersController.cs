using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SlidersController : Controller
    {
        private readonly Models.AppContext _context;
        private readonly IWebHostEnvironment _environment;

        public SlidersController(Models.AppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/Sliders
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Slider.Include(s => s.City);
			ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name");
			return View(await appContext.ToListAsync());
        }

        // GET: Admin/Sliders/Create
        public IActionResult Create()
        {
            ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoverImage, Alias, CityID")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                if (slider.CoverImage != null && slider.CoverImage.Length > 0)
                {
                    // Kiểm tra dung lượng tệp tải lên
                    if (slider.CoverImage.Length <= 10 * 1024 * 1024) // 10MB
                    {
                        string folder = "Uploads/Sliders";
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(slider.CoverImage.FileName);
                        string filePath = Path.Combine(_environment.WebRootPath, folder, uniqueFileName);

                        // Tạo thư mục nếu không tồn tại
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));

                        // Lưu tệp tải lên vào máy chủ
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await slider.CoverImage.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn đến tệp tải lên
                        slider.Image = "/" + folder + "/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("CoverImage", "Dung lượng tệp tải lên quá lớn (tối đa 10MB).");
                        return View(slider);
                    }
                }
				_context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Set<City>(), "Id", "Id", slider.CityID);
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Slider == null)
            {
                return Problem("Entity set 'AppContext.Slider'  is null.");
            }
            var slider = await _context.Slider.FindAsync(id);
            if (slider != null)
            {
				string fullPath = Path.Combine(_environment.WebRootPath, slider.Image.TrimStart('/'));

				if (System.IO.File.Exists(fullPath))
				{
					System.IO.File.Delete(fullPath);
				}
				_context.Slider.Remove(slider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
          return (_context.Slider?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
