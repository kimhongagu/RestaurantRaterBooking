using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostCategoriesController : Controller
    {
        private readonly Models.AppContext _context;

        public PostCategoriesController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Admin/PostCategories
        public async Task<IActionResult> Index()
        {
              return _context.PostCategory != null ? 
                          View(await _context.PostCategory.ToListAsync()) :
                          Problem("Entity set 'AppContext.PostCategory'  is null.");
        }

        // GET: Admin/PostCategories/Create
        public IActionResult Create()
        {
            return View();
        }

		// POST: Admin/PostCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name, PostType")] PostCategory postCategory)
		{
			if (ModelState.IsValid)
			{
				_context.Add(postCategory);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(postCategory);
		}

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PostCategory == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategory.FindAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }
            return View(postCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] PostCategory postCategory)
        {
            if (id != postCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCategoryExists(postCategory.Id))
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
            return View(postCategory);
        }


        // POST: Admin/PostCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PostCategory == null)
            {
                return Problem("Entity set 'AppContext.PostCategory'  is null.");
            }
            var postCategory = await _context.PostCategory.FindAsync(id);
            if (postCategory != null)
            {
                _context.PostCategory.Remove(postCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostCategoryExists(Guid id)
        {
          return (_context.PostCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
