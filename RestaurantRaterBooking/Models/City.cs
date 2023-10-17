namespace RestaurantRaterBooking.Models
{
    public class City
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }

        public ICollection<Slider> Sliders { get; set; }
    }
}
