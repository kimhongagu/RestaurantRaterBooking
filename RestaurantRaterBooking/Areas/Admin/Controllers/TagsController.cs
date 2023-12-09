using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using X.PagedList;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private readonly Models.AppContext _context;

        public TagsController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Admin/Tags
        public async Task<IActionResult> Index(string searchText, int? page)
        {
			int pageSize = 5;
			int pageNumber = page ?? 1;

			IEnumerable<Tag> appContext = _context.Tag;

			if (!string.IsNullOrEmpty(searchText))
			{
				appContext = appContext.Where(x => x.Name.Contains(searchText)|| x.TagType.ToString().Contains(searchText));
			}

			var pagedList = await appContext.ToPagedListAsync(pageNumber, pageSize);
			return View(pagedList);
		}

        // GET: Admin/Tags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Tag == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Admin/Tags/Create
        public IActionResult Create()
        {
			ViewBag.TagTypes = Enum.GetValues(typeof(TagType))
							 .Cast<TagType>()
							 .Select(e => new SelectListItem
							 {
								 Value = e.ToString(),
								 Text = e.ToString()
							 })
							 .ToList();
			return View();
        }

        // POST: Admin/Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TagType")] Tag tag)
        {
            ViewBag.TagTypes = Enum.GetValues(typeof(TagType))
                             .Cast<TagType>()
                             .Select(e => new SelectListItem
                             {
                                 Value = e.ToString(),
                                 Text = e.ToString()
                             })
                             .ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    tag.Id = Guid.NewGuid();
                    _context.Add(tag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(tag);
                }
            }
            return View(tag);
        }

        // GET: Admin/Tags/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
			if (id == null || _context.PostCategory == null)
			{
				return NotFound();
			}

			var tag = await _context.Tag.FindAsync(id);
			if (tag == null)
			{
				return NotFound();
			}
			return View(tag);
		}

        // POST: Admin/Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,TagType")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            return View(tag);
        }

        // GET: Admin/Tags/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Tag == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Admin/Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Tag == null)
            {
                return Problem("Entity set 'AppContext.Tag'  is null.");
            }
            var tag = await _context.Tag.FindAsync(id);
            if (tag != null)
            {
                _context.Tag.Remove(tag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(Guid id)
        {
          return (_context.Tag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
