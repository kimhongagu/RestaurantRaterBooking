using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantRaterBooking.Models
{
    public class Slider
    {
        public int Id { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? CoverImage { get; set; }

        public string Alias { get; set; }

        public Guid? CityID { get; set; }
        public virtual City? City { get; set; }
    }
}
