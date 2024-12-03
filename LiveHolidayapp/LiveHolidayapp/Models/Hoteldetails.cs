namespace LiveHolidayapp.Models
{
    public class Hoteldetails
    {
        public int Hotelcodes { get; set; }
        public string Language { get; set; }
    }

    public class Hoteldetailsresponse
    {
        public HoteldetailStatus Status { get; set; }
        public List<Hoteldetail> HotelDetails { get; set; }
    }

    public class HoteldetailStatus
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class Hoteldetail
    {
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public string Description { get; set; }
        public List<string> HotelFacilities { get; set; }
        public Attractions Attractions { get; set; }
        public List<string> Images { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string CityId { get; set; }
        public string CountryName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Map { get; set; }
        public int HotelRating { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
    }

    public class Attractions
    {
        public string _1 { get; set; }
    }
}
