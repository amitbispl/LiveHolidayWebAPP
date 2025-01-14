using System.Xml.Linq;

namespace LiveHolidayapp.Models
{
    public class M_Company
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string CMEmailID { get; set; }
        public string CMMobileNo { get; set; }
        public string AppDomain { get; set; }
        public string APIDomain { get; set; }
        public string Logo { get; set; }
        public string CompanyId { get; set; }
        public string APIURL { get; set; }
        public string Token { get; set; }
        public string TWLink { get; set; }
        public string WALink { get; set; }
        public string INLink { get; set; }
        public string FBLink { get; set; }
        public string URL { get; set; }
        public string ValidatyTODate { get; set; }
        public string ValidatyFormDate { get; set; }
        public string RedirectDomain { get; set; }
        public int StartAfterday { get; set; }
        public string ISchild { get; set; }
        public int DayAfter { get; set; }
        public string PriceRangeStart { get; set; }
        public string PriceRangeEnd { get; set; }
        public string Theme { get; set; }
        public string HotelListTheme { get; set; }

    }
}
