using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RestaurantRaterBooking.Areas.RestaurantUser.Controllers
{
	[Area("RestaurantUser")]
	[Authorize(Roles = "Restaurant")]
	public class HomeController : Controller
	{
		private readonly Models.AppContext _context;

		public HomeController(Models.AppContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			//var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
			//var uId = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
			//ViewData["NumOfBooking"] = _context.Booking.Where()
			return View();
		}
	}
}
