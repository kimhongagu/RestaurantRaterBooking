namespace RestaurantRaterBooking.Models
{
    public class RestaurantTag
    {
        public Guid Id { get; set; }

        public Guid? RestaurantID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public Guid TagID { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
