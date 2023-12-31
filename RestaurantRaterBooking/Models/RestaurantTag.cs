﻿namespace RestaurantRaterBooking.Models
{
	public class RestaurantTag
	{
		public Guid Id { get; set; }
		public Guid RestaurantId { get; set; }
		public virtual Restaurant Restaurant { get; set; }
		public Guid TagId { get; set; }
		public virtual Tag Tag { get; set; }
	}
}
