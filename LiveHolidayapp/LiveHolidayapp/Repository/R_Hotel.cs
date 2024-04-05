﻿using LiveHolidayapp.Models;
using Newtonsoft.Json;
using NuGet.Common;
using System.Security.Policy;

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

        public string HotelsearchResponse(M_SearchHotel req,string Token)
        {
            string resp = "";
            try
            {
                var detail = JsonConvert.SerializeObject(req);
                 resp = general.CallPostFunction(detail, Token, "hotel/SearchHotel");
            }
            catch(Exception ex)
            {

            }
            return resp;
        }

        public string PropertyDetail(int Hotelid)
        {
            string resp = "";
            try
            {
                string URL = "https://content.rezlive.com/api/propertyDetail/F89880940CF07BF1D5DF4A4C5CA0D08B/" + Hotelid + "";
                resp = general.invokeGetRequest(URL);
            }
            catch (Exception ex)
            {

            }
            return resp;
        }



    }
}
