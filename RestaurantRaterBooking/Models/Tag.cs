namespace RestaurantRaterBooking.Models
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

		public TagType TypeTag { get; set; }

		public Guid? RestaurantId { get; set; }
		public virtual Restaurant? Restaurant { get; set; }

		public Guid? BlogId { get; set; }
		public virtual Blog? Blog { get; set; }

		public Guid? NewsId { get; set; }
		public virtual News? News { get; set; }
	}

	public enum TagType
	{
		Restaurant,
		Blog,
		News
	}
}
