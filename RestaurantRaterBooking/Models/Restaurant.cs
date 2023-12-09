using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantRaterBooking.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "Tên nhà hàng";

        public string Description { get; set; } = "Mô tả chi tiết";

        public string Address { get; set; } = "Địa chỉ nhà hàng";

        public string Phone { get; set; } = "Số điện thoại";

        public string Website { get; set; } = "Website";

        public string Email { get; set; } = "Email";

        public string Offer { get; set; } = "Các ưu đãi";

        public TimeSpan OpeningHour { get; set; } = default;

        public TimeSpan ClosingHour { get; set; } = default;

        public string SuitableFor { get; set; } = "Phù hợp với?";

        public string SpecialFeature { get; set; } = "Điểm đặc trưng";

        public string Space { get; set; } = "Không gian";

        public string ParkingSpace { get; set; } = "Chỗ đỗ xe";

        public string Note { get; set; } = "Lưu ý";

        public string SpecialDish { get; set; } = "Món đặc trưng";

        [DisplayName("Ghế trẻ em")]
        public bool ChildrenChair { get; set; }
        public bool Wifi { get; set; }
        [DisplayName("Điều hòa")]
        public bool AirConditioning { get; set; }
        [DisplayName("Visa / Master card")]
        public bool VisaMasterCard { get; set; }
        [DisplayName("Hóa đơn VAT")]
        public bool VATInvoice { get; set; }
        [DisplayName("Phòng riêng")]
        public bool PrivateRoom { get; set; }
        [DisplayName("Bàn ngoài trời")]
        public bool OutdoorTable { get; set; }
        [DisplayName("Khu trẻ em")]
        public bool ChildrenPlayArea { get; set; }
        [DisplayName("Khu vực hút thuốc")]
        public bool SmokiingArea { get; set; }
        [DisplayName("Hóa đơn trực tiếp")]
        public bool DirectBill { get; set; }
        public bool Karaoke { get; set; }
        [DisplayName("Máy chiếu")]
        public bool Projector { get; set; }
        [DisplayName("Trang trí sự kiện")]
        public bool EnventDecoration { get; set; }
        [NotMapped]
		public double AverageRating { get; set; }

		public Guid? CityID { get; set; } = default;
        public virtual City? City { get; set; } = default;

        public Guid CategoryID { get; set; } = default;
        public virtual Category? Category { get; set; } = default;


        public ICollection<Review>? Reviews { get; set; }

        public ICollection<Image>? Images { get; set; } = new List<Image>();

		public ICollection<Booking>? Bookings { get; set; }

		//public ICollection<Tag>? Tags { get; set; }
		public ICollection<RestaurantTag>? RestaurantTags { get; set; }

	}
}
