namespace LiveHolidayapp.Models
{
    public class M_Hotel
    {
        public List<M_countrylist> countryList { get; set; }
        public SearchHotel searchHotel { get; set; }
        public List<HotelsearchResponse> hotelsearchResponses { get; set; }
        public M_SearchHotel m_SearchHotel { get; set; }

        public List<StarRating> starRatings { get; set; }
    }

    public class SearchHotel
    {
        public string txtHotelCheckOut { get; set; }
        public string txtHotelCheckIn { get; set; }
        public string hdnCountry { get; set; }
        public string hdnHotelCity { get; set; }
        public string hdnNatinality { get; set; }
        public string ddlChild { get; set; }
        public string ddlAdult { get; set; }
    }


    public class M_SearchHotel
    {
        public string txtHotelCheckOut { get; set; }
        public string txtHotelCheckIn { get; set; }
        public string hdnCountry { get; set; }
        public string hdnHotelCity { get; set; }
        public string hdnNatinality { get; set; }
        public string ddlChild { get; set; }
        public string ddlAdult { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    

    public class HotelsearchResponse
    {
        public string id { get; set; }
        public string hotelName { get; set; }
        public string hotelResults_ID { get; set; }
        public string hotelCode { get; set; }
        public string hotelPicture { get; set; }
        public string starRating { get; set; }
        public string price { get; set; }
    }

    public class StarRating
    {
        public string Hotelid { get; set; }
        public string Rating { get; set; }
    }


}
