namespace LiveHolidayapp.Models
{
    public class M_Hotelreport
    {
        public int ID { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NoOfRooms { get; set; }
        public DateTime BookingDate { get; set; }
        public int FormNo { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public decimal BookingAmount { get; set; }
        public string HotelBookingRefNo { get; set; }
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string PackageName { get; set; }
        public string BookStatus { get; set; }
        public string bookingfile { get; set; }
        public List<PassengerDetails> PassengerDetails { get; set; }
    }
    public class PassengerDetails
    {
        public string Name { get; set; }
        public string PaxType { get; set; }
    }
    public class HotelReq
    {
        public int Formno { get; set; }
    }
    public class BookHotelReport
    {
        public List<M_Hotelreport> Hotelreports { get; set; }
    }
}
