using Newtonsoft.Json;
using X.PagedList;

namespace LiveHolidayapp.Models
{
    public class M_Hotel
    {
        public List<M_countrylist> countryList { get; set; }
        public SearchHotel searchHotel { get; set; }
        public List<HotelsearchResponse> hotelsearchResponses { get; set; }
        public M_SearchHotel m_SearchHotel { get; set; }
        public List<StarRating> starRatings { get; set; }
        public IPagedList<HotelsearchResponse> Hotelpaging { get; set; }
        public List<Image> images { get; set; }
        public string[] Amenities { get; set; }
        public List<M_PackageTypes> PackageTypesList { get; set; }

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
        public string Orderid { get; set; } 
        public int IDWiseDayAfter { get; set; }
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

    public class PropertyDetailRoot
    {
        public bool success { get; set; }
        public PropertyDetail propertyDetail { get; set; }
    }

    public class PropertyDetail
    {
        public int property_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string postal_code { get; set; }
        public string city_code { get; set; }
        public string city_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string rating { get; set; }
        public int status { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string phone { get; set; }
        public string category { get; set; }
        public string chain_name { get; set; }
        public CheckinDetails checkin { get; set; }
        public string checkout_time { get; set; }
        public List<Image> images { get; set; }
        [JsonProperty("Amenities")]
        public string[] Amenities { get; set; }
        public Description Description { get; set; }
    }

    public class CheckinDetails
    {
        public string begin_time { get; set; }
        public string instruction { get; set; }
        public string special_instructions { get; set; }
        public string min_age { get; set; }
    }

    public class Image
    {
        public string image_tag { get; set; }
        public ImageUrl image_url { get; set; }
    }

    public class ImageUrl
    {
        [JsonProperty("350px")]
        public Uri _350Px { get; set; }

        [JsonProperty("70px")]
        public Uri _70Px { get; set; }

        [JsonProperty("1000px")]
        public Uri _1000Px { get; set; }
    }

    public class Description
    {
        [JsonProperty("amenities")]
        public string Amenities { get; set; }

        [JsonProperty("dining")]
        public string Dining { get; set; }

        [JsonProperty("business_amenities")]
        public string BusinessAmenities { get; set; }

        [JsonProperty("rooms")]
        public string Rooms { get; set; }

        [JsonProperty("attractions")]
        public string Attractions { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }

    public class M_PackageTypes
    {
        public string OrderId { get; set; }
        public int dayafter { get; set; }
        public string package { get; set; }
    }
}
