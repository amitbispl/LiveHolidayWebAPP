using LiveHolidayapp.Models;
using Newtonsoft.Json;
using NuGet.Common;

namespace LiveHolidayapp.Repository
{
    public class R_Hotel
    {
        General general = new General();
        public List<M_countrylist> CountryList(CountrylistREQ countrylistREQ,string Token)
        {
            List<M_countrylist> obj = new List<M_countrylist>();
            try
            {
                var detail = JsonConvert.SerializeObject(countrylistREQ);
                var resp =  general.CallPostFunction(detail, Token, "hotel/countrylist");
                var output = JsonConvert.DeserializeObject<CommonResponse<List<M_countrylist>>>(resp);
                obj = output.Data;
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public List<Citylist> GetCitylist(Citylistreq req, string Token)
        {
            List<Citylist> obj = new List<Citylist>();
            try
            {
                var detail = JsonConvert.SerializeObject(req);
                var resp = general.CallPostFunction(detail, Token, "hotel/citylist");
                var output = JsonConvert.DeserializeObject<CommonResponse<List<Citylist>>>(resp);
                obj = output.Data;
            }
            catch
            {

            }
            return obj;
        }

        public List<HotelsearchResponse> HotelsearchResponse(M_SearchHotel req,string Token)
        {
            List<HotelsearchResponse> res = new List<HotelsearchResponse>();
            try
            {
                var detail = JsonConvert.SerializeObject(req);
                var resp = general.CallPostFunction(detail, Token, "hotel/SearchHotel");
                var output = JsonConvert.DeserializeObject<CommonResponse<List<HotelsearchResponse>>>(resp);
                res = output.Data;
            }
            catch
            {

            }
            return res;
        }


    }
}
