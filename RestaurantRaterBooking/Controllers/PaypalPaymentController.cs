using Microsoft.AspNetCore.Mvc;
using RestaurantRaterBooking.Models;
using RestaurantRaterBooking.Services;

namespace RestaurantRaterBooking.Controllers
{
    public class PaypalPaymentController : Controller
    {
        private readonly ILogger<PaypalPaymentController> _logger;
        private readonly IUnitOfWork _unitOfWork;
		private readonly Models.AppContext _context;

		public PaypalPaymentController(ILogger<PaypalPaymentController> logger, IUnitOfWork unitOfWork, Models.AppContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success(string paymentId, string token, string PayerID)
        {
            ViewData["PayemntId"] = paymentId;
            ViewData["token"] = token;
            ViewData["payid"] = PayerID;
			if (TempData.ContainsKey("bookingId") && TempData["bookingId"] is Guid bookingId)
            {
				var booking = _context.Booking.Find(ViewBag.BookingId = bookingId);
                booking.Status = Status.WaitingList;
                _context.Update(booking);
                _context.SaveChanges();
			}
			return View();
        }

        [HttpPost]
        public async Task<ActionResult> PayUsingCard(PaymentVM model)
        {
            try
            {
                if(model.Amount == 0)
                {
                    TempData["error"] = "please enter amount";
                    return RedirectToAction(nameof(Index));
                }

                // generate Paypal payment details
                decimal amount = model.Amount;
                string returnUrl = "https://localhost:7116/PaypalPayment/Success"; // specify your return url
                string cancelUrl = "https://localhost:7116/PaypalPayment/Cancel"; // specify your cancal url

                // create Paypal payment order
                var createdPayment = await _unitOfWork.PaypalServices.CreateOrderAsync(amount, returnUrl, cancelUrl);

                // get the Paypal approval url
                string approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.ToLower() == "approval_url")?.href;

                // redirect the user to the Paypal approval ur;
                if (!string.IsNullOrEmpty(approvalUrl))
                {
                    TempData["bookingId"] = model.bookingId;
                    return Redirect(approvalUrl);
                }
                else
                {
                    TempData["error"] = "Failed to initiate Paypal payment.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
