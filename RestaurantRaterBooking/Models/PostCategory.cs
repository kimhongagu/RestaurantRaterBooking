namespace RestaurantRaterBooking.Models
{
    public class PostCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PostType PostType { get; set; }

        public ICollection<Blog>? Blog { get; set; }

        public ICollection<News>? News { get; set; }
    }

    public enum PostType
    {
        News,
        Blog
    }
}
