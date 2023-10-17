namespace RestaurantRaterBooking.Models
{
    public class PostCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Blog> Blog { get; set; }

        public ICollection<News> News { get; set; }
    }
}
