namespace RestaurantRaterBooking.Models
{
    public class Review
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime EditedAt { get; set; }

        public Guid? RestaurantID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public string? UserID { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
