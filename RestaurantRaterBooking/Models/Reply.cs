namespace RestaurantRaterBooking.Models
{
    public class Reply
    {
        public Guid Id { get; set; }

        public string ReplyComment { get; set; }

        public DateTime ReplyAt { get; set; }

        public Guid? ReviewID { get; set; }
        public virtual Review? Review { get; set; }
    }
}
