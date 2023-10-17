namespace RestaurantRaterBooking.Models
{
    public class News
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ShortContent { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public bool IsPublish { get; set; }

        public bool IsHot { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime EđitedAt { get; set; }

        public string EditedBy { get; set; }

        public Guid? PostCategoryID { get; set; }
        public virtual PostCategory? PostCategory { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; }
    }
}
