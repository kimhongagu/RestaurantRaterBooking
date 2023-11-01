namespace RestaurantRaterBooking.Models
{
	public class NewsTag
	{
		public Guid Id { get; set; }
		public Guid NewsId { get; set; }
		public virtual News News { get; set; }
		public Guid TagId { get; set; }
		public virtual Tag Tag { get; set; }
	}
}
