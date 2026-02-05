namespace LiveHolidayapp.Models
{
    public class M_SaveHotelDetail
    {
        public string Hotelid { get; set; }
        public string HotelName { get; set; }
        public string cityid { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string Description { get; set; }
        public string Natinality { get; set; }
        public List<string> Aminities { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<HotelCodes> Hotelcodes { get; set; }
    }
}
