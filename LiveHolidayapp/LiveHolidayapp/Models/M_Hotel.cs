namespace LiveHolidayapp.Models
{
    public class M_Hotel
    {
        public List<M_countrylist> countryList { get; set; }
        public SearchHotel searchHotel { get; set; }
    }

    public class SearchHotel
    {
        public string txtHotelCheckOut { get; set; }
        public string txtHotelCheckIn { get; set; }
        public string hdnCountry { get; set; }
        public string hdnHotelCity { get; set; }
        public string hdnNatinality { get; set; }
        public string ddlChil { get; set; }
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
    }

    public class HotelsearchResponse
    {

    }

}
