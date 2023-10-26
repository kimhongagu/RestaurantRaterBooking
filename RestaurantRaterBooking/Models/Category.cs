using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantRaterBooking.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? CoverImage { get; set; }

        public ICollection<Restaurant>? Restaurants { get; set; }
    }
}
