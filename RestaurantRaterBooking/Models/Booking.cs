namespace RestaurantRaterBooking.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public DateTime BookingDate { get; set; }

        public DateTime BookingTime { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public int? Adults { get; set; }

        public int? Children { get; set; }

        public string? Note { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? RestaurantID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

		public string? UserID { get; set; }
		public virtual ApplicationUser? User { get; set; }
	}

    public enum Status
    {
        Unpaid,
        WaitingList,
        AutoCanceled,
        CanceledByRestaurant,
        CanceledByCustomer,
        Confirmed,
        Unsuccessful,
        Completed
    }
}
