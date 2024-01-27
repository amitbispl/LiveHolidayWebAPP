using System.Xml.Linq;

namespace LiveHolidayapp.Models
{
    public class M_Company
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string CompanyName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string StartAfterday { get; set; }
        public string DayAfter { get; set; }
        public string PriceRangeStart { get; set; }
        public string PriceRangeEnd { get; set; }
        public string ISChild { get; set; }
        public string IsOfflineBook { get; set; }
        public string IsAdditionalService { get; set; }
        public string IsInternational { get; set; }
        public string IsCustomizeDuration { get; set; }
        public string Theme { get; set; }

    }
}
