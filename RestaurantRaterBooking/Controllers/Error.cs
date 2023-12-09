using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantRaterBooking.Models;

namespace RestaurantRaterBooking.Controllers
{
    public class Error : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Error1(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            return View(new ErrorViewModel { StatusCode = statusCode, OriginalPath = feature?.OriginalPath });
        }
    }
}
