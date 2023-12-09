using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategoriesController : Controller
	{
		private readonly Models.AppContext _context;
		private readonly IWebHostEnvironment _environment;

		public CategoriesController(Models.AppContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		// GET: Admin/Categories
		public async Task<IActionResult> Index()
		{
			return _context.Category != null ?
						View(await _context.Category.ToListAsync()) :
						Problem("Entity set 'AppContext.Category'  is null.");
		}

		// GET: Admin/Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name, CoverImage")] Category category)
		{
			if (ModelState.IsValid)
			{
				if (category.CoverImage != null && category.CoverImage.Length > 0)
				{
					// Kiểm tra dung lượng tệp tải lên
					if (category.CoverImage.Length <= 10 * 1024 * 1024) // 10MB
					{
						string folder = "Uploads/Categories";
						string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(category.CoverImage.FileName);
						string filePath = Path.Combine(_environment.WebRootPath, folder, uniqueFileName);

						// Tạo thư mục nếu không tồn tại
						Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));

						// Lưu tệp tải lên vào máy chủ
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await category.CoverImage.CopyToAsync(stream);
						}

						// Cập nhật đường dẫn đến tệp tải lên
						category.Image = "/" + folder + "/" + uniqueFileName;
					}
					else
					{
						ModelState.AddModelError("CoverImage", "Dung lượng tệp tải lên quá lớn (tối đa 10MB).");
						return View(category);
					}
				}
				category.Id = Guid.NewGuid();
				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		// GET: Admin/Categories/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.Category == null)
			{
				return NotFound();
			}

			var category = await _context.Category.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Admin/Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [FromForm] Category category, [FromForm]IFormFile CoverImage)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					if (category.CoverImage != null && category.CoverImage.Length > 0)
					{
						// Kiểm tra dung lượng tệp tải lên
						if (category.CoverImage.Length <= 10 * 1024 * 1024) // 10MB
						{
							string folder = "Uploads/Categories";
							string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(category.CoverImage.FileName);
							string filePath = Path.Combine(_environment.WebRootPath, folder, uniqueFileName);

							// Tạo thư mục nếu không tồn tại
							Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));

							// Lưu tệp tải lên vào máy chủ
							using (var stream = new FileStream(filePath, FileMode.Create))
							{
								await category.CoverImage.CopyToAsync(stream);
							}

							// Cập nhật đường dẫn đến tệp tải lên
							category.Image = "/" + folder + "/" + uniqueFileName;
						}
						else
						{
							ModelState.AddModelError("CoverImage", "Dung lượng tệp tải lên quá lớn (tối đa 10MB).");
							return View(category);
						}
					}
					_context.Update(category);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoryExists(category.Id))
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
			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			try
			{
				var category = await _context.Category.FindAsync(id);
				if (category == null)
				{
					return NotFound();
				}

				if (!string.IsNullOrEmpty(category.Image))
				{
					string imagePath = category.Image.Replace("/", "\\");

					string fullPath = _environment.WebRootPath + imagePath;

					if (System.IO.File.Exists(fullPath))
					{
						System.IO.File.Delete(fullPath);
					}
				}

				_context.Category.Remove(category);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during SaveChangesAsync(): {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		private bool CategoryExists(Guid id)
		{
			return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
