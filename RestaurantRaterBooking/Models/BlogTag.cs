namespace RestaurantRaterBooking.Models
{
    public class BlogTag
    {
        public Guid Id { get; set; }

        public Guid? BlogID { get; set; }
        public virtual Blog? Blog { get; set; }

        public Guid TagID { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
