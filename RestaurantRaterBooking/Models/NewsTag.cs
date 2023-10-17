namespace RestaurantRaterBooking.Models
{
    public class NewsTag
    {
        public Guid Id { get; set; }

        public Guid? NewsID { get; set; }
        public virtual News? News { get; set; }

        public Guid TagID { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
