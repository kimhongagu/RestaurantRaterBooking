namespace RestaurantRaterBooking.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public DateOnly BookingDate { get; set; }

        public TimeOnly BookingTime { get; set; }

        public int Adults { get; set; }

        public int Children { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DepositStatus { get; set; }

        public decimal DepositAmount { get; set; }

        public Guid? RestaurantID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public int? UserID { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
