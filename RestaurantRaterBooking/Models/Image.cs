namespace RestaurantRaterBooking.Models
{
    public class Image
    {
        public Guid Id { get; set; }

        public string ImagePath {  get; set; }

        public string? Alias { get; set; }

        public Guid? RestaurantID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

		public ImageType ImageType { get; set; }
	}

	public enum ImageType
	{
		RestaurantImage,
		MenuImage
	}
}
