namespace RestaurantRaterBooking.Models
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

		public TagType TagType { get; set; }

		public ICollection<RestaurantTag>? RestaurantTags { get; set; }
		public ICollection<NewsTag>? NewsTags { get; set; }
		public ICollection<BlogTag>? BlogTags { get; set; }

	}
	public enum TagType
	{
		Restaurant,
		News,
		Blog
	}
}
