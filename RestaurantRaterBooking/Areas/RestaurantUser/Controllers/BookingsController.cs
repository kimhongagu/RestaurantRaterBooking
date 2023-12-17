using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Areas.RestaurantUser.Controllers
{
	[Area("RestaurantUser")]
	[Authorize(Roles = "Restaurant")]
	public class BookingsController : Controller
	{
		private readonly Models.AppContext _context;

		public BookingsController(Models.AppContext context)
		{
			_context = context;
		}

		// GET: RestaurantUser/Bookings
		public async Task<IActionResult> Index(Guid restaurantId)
		{
			if (TempData.ContainsKey("restaurantId") && TempData["restaurantId"] is Guid resId)
			{
				restaurantId = resId;
			}
			var appContext = _context.Booking.Include(b => b.User)
			.Where(r => r.RestaurantID == restaurantId && r.Status != Status.Unpaid && r.Status != Status.AutoCanceled);
			return View(await appContext.ToListAsync());
		}

		public async Task<IActionResult> UpdateStatus(Guid bookingId, string status, Guid restaurantId)
		{
			try
			{
				var booking = await _context.Booking.FindAsync(bookingId);

				if (booking == null)
				{
					return NotFound();
				}

				// Cập nhật trạng thái của đơn đặt bàn
				if (status == "Confirmed")
				{
					booking.Status = Status.Confirmed;
				}
				else if (status == "Completed")
				{
					booking.Status = Status.Completed;
				}
				else if (status == "Unsuccessful")
				{
					booking.Status = Status.Unsuccessful;
				}

				_context.Update(booking);
				await _context.SaveChangesAsync();
				TempData["restaurantId"] = restaurantId;
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error updating status: {ex.Message}");
			}
		}

		private bool BookingExists(Guid id)
		{
			return (_context.Booking?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
