namespace LiveHolidayapp.Models
{
    public class M_HotelBook
    {
        public string userName { get; set; }
        public string hotelCode { get; set; }
        public string hotelName { get; set; }
        public string checkOutDate { get; set; }
        public string checkInDate { get; set; }
        public string noOfRoom { get; set; }
        public string formNo { get; set; }
        public string companyId { get; set; }
        public string cityName { get; set; }
        public string address { get; set; }
        public string userCity { get; set; }
        public string mobileNo { get; set; }
        public string emailID { get; set; }
        public string bookingAmount { get; set; }
        public string orderId { get; set; }
        public string adultCount { get; set; }
        public string childCount { get; set; }
        public string dob { get; set; }
        public List<Hotelbookpassenger> hotelBookPassengers { get; set; }
    }

    public class Hotelbookpassenger
    {
        public string dob { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string title { get; set; }
        public string paxType { get; set; }
        public string age { get; set; }
    }

}
