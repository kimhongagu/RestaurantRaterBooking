using Microsoft.AspNetCore.Mvc;

namespace RestaurantRaterBooking.Areas.RestaurantUser.Controllers
{
	[Area("RestaurantUser")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
