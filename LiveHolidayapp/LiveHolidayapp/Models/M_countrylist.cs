using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LiveHolidayapp.Models
{
    public class M_countrylist
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }



    public class CountrylistREQ
    {
        public string countryName { get; set; }
        public int registerId { get; set; }
    }

    public class Citylist
    {
        public int id { get; set; }
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string countryCode { get; set; }
        public object activeStatus { get; set; }
    }

    public class Citylistreq
    {
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public int registerId { get; set; }
    }



}
