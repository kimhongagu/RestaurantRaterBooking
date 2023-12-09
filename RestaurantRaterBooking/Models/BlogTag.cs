namespace RestaurantRaterBooking.Models
{
	public class BlogTag
	{
		public Guid Id { get; set; }
		public Guid BlogId { get; set; }
		public virtual Blog Blog { get; set; }
		public Guid TagId { get; set; }
		public virtual Tag Tag { get; set; }
	}
}
