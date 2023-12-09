using Microsoft.AspNetCore.Mvc;
using RestaurantRaterBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RestaurantRaterBooking.Areas.Identity.Pages.Account.Manage;

namespace RestaurantRaterBooking.Controllers
{
    public class BookingsController : Controller
    {
        private readonly Models.AppContext _context;

        public BookingsController(Models.AppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
            var bookings = await _context.Booking.Include(r=> r.Restaurant).Where(u => u.UserID == userId).ToListAsync();
            ViewData["BookingList"] = bookings;
            return View();
        }
    }
}
