using LiveHolidayapp.Models;
using Newtonsoft.Json;

namespace LiveHolidayapp.Repository
{
    public class R_Hotel
    {
        General general = new General();
        public async Task<List<M_countrylist>> CountryList(CountrylistREQ countrylistREQ,string Token)
        {
            List<M_countrylist> obj = new List<M_countrylist>();
            try
            {
                var detail = JsonConvert.SerializeObject(countrylistREQ);
                var resp = await general.CallPostFunction(detail, Token, "hotel/countrylist");
                var output = JsonConvert.DeserializeObject<CommonResponse<List<M_countrylist>>>(resp);
                obj = output.Data;
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
    }
}
