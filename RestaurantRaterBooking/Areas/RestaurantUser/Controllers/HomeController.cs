using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantRaterBooking.Areas.RestaurantUser.Controllers
{
	[Area("RestaurantUser")]
	[Authorize(Roles = "Restaurant")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
