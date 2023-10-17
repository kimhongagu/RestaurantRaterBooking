using System.ComponentModel;

namespace RestaurantRaterBooking.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public TimeOnly OpeningHour { get; set; }

        public TimeOnly ClosingHour { get; set; }

        public string SuitableFor { get; set; }

        public string SpecialFeature { get; set; }

        public string Space { get; set; }

        public string ParkingSpace { get; set; }

        public bool ChildrenChair { get; set; }
        public bool Wifi { get; set; }
        public bool AirConditioning { get; set; }
        public bool VisaMasterCard { get; set; }
        public bool VATInvoice { get; set; }
        public bool PrivateRoom { get; set; }
        public bool OutdoorTable { get; set; }
        public bool ChildrenPlayArea { get; set; }
        public bool SmokiingArea { get; set; }
        public bool DirectBill { get; set; }
        public bool Delivery { get; set; }
        public bool Karaoke { get; set; }
        public bool Projector { get; set; }
        public bool EnventDecoration { get; set; }

        public Guid? CityID { get; set; }
        public virtual City? City { get; set; } 

        public Guid CategoryID { get; set; }
        public virtual Category? Category { get; set; }


        public ICollection<Review> Reviews { get; set; }

        public ICollection<Image> Images { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<RestaurantTag> RestaurantTags { get; set; }
    }
}
