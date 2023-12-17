namespace RestaurantRaterBooking.Models
{
    public class PaymentVM
    {
        public decimal Amount { get; set; }
        public Guid bookingId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
