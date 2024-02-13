using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LiveHolidayapp.Controllers
{
    public class LiveHotelController : Controller
    {
        CompanyDetail _companyDetail;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string Theme = "";
        M_Company obj = new M_Company();
        R_Hotel _Hotel = new R_Hotel();
        private static string LastCountryname = "";
        public LiveHotelController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            obj = this._companyDetail.GetCompany();
            Theme = obj.Theme;
        }

        public IActionResult SearchHotel()
        {

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {

                M_Hotel obj = new M_Hotel();
                //get country list-----------------
                CountrylistREQ creq = new CountrylistREQ();
                creq.countryName = "";
                creq.registerId = Convert.ToInt32(HttpContext.Session.GetString("registerId"));
                List<M_countrylist> countryList = new List<M_countrylist>();
                countryList = _Hotel.CountryList(creq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                HttpContext.Session.SetComplexData("countryList", countryList);
                obj.countryList = countryList;
                obj.searchHotel = new SearchHotel();
                obj.searchHotel.hdnCountry = countryList[0].CountryName;
                obj.searchHotel.hdnNatinality = countryList[0].Code;
                //--get citylist for first country
                Citylistreq cityreq = new Citylistreq();
                cityreq.cityName = "";
                cityreq.countryCode = countryList[0].Code;
                cityreq.registerId = Convert.ToInt32(HttpContext.Session.GetString("registerId"));
                List<Citylist> citylist = new List<Citylist>();
                citylist = _Hotel.GetCitylist(cityreq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                HttpContext.Session.SetComplexData("citylist", citylist);
                LastCountryname = countryList[0].Code;
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/LiveHotel/SearchHotel.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/LiveHotel/SearchHotel.cshtml", obj);
                }
            }
            else
            {
                string returnUrl = Url.Action("SearchHotel", "LiveHotel")!;
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
        }

        [HttpPost]
        public IActionResult GetCountry(string CountryName)
        {
            List<M_countrylist> data = HttpContext.Session.GetComplexData<List<M_countrylist>>("countryList");
            var result = data.Where(p => Regex.IsMatch(p.CountryName, CountryName, RegexOptions.IgnoreCase)).ToList();
            return Json(result);
        }

        [HttpPost]
        public IActionResult GetCitylist(string Cityname, string countrycode)
        {
            List<Citylist> filterdata = new List<Citylist>();
            if (LastCountryname == countrycode)
            {
                List<Citylist> data = HttpContext.Session.GetComplexData<List<Citylist>>("citylist");
                filterdata = data.Where(p => Regex.IsMatch(p.cityName, Cityname, RegexOptions.IgnoreCase)).ToList();
            }
            else
            {
                Citylistreq cityreq = new Citylistreq();
                cityreq.cityName = Cityname;
                cityreq.countryCode = countrycode;
                cityreq.registerId = Convert.ToInt32(HttpContext.Session.GetString("registerId"));
                List<Citylist> citylist = new List<Citylist>();
                citylist = _Hotel.GetCitylist(cityreq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                HttpContext.Session.SetComplexData("citylist", citylist);
                LastCountryname = countrycode;
                filterdata = citylist.Where(p => Regex.IsMatch(p.cityName, Cityname, RegexOptions.IgnoreCase)).ToList();
            }
            return Json(filterdata);
        }

        [HttpPost]
        public IActionResult SearchHotel(M_SearchHotel Hotelreq)
        {
            string msg = string.Empty;
            M_Hotel obj = new M_Hotel();
            try
            {
                List<HotelsearchResponse> list = new List<HotelsearchResponse>();
                var response = _Hotel.HotelsearchResponse(Hotelreq, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var output = JsonConvert.DeserializeObject<CommonResponse<List<HotelsearchResponse>>>(response);
                if (output != null)
                {
                    if (output.Message == "Success")
                    {
                        obj.m_SearchHotel = new M_SearchHotel();
                        obj.m_SearchHotel = Hotelreq;
                        list = output.Data;
                        obj.hotelsearchResponses = list;
                        HttpContext.Session.SetComplexData("hotelsearchResponses", obj);
                        msg = "Success";
                    }
                    else
                    {
                        msg = output.Message;
                    }

                }
                else
                {
                    msg = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                msg = "Something went wrong";
            }
            return Json(new { msg });
        }

        public IActionResult Roomlist()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                M_Hotel obj = new M_Hotel();

                var PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
                var PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                var hotelfilter = result.hotelsearchResponses.Where(p=>Convert.ToDecimal(p.price)>=PriceRangeStart && Convert.ToDecimal(p.price)<=PriceRangeEnd).ToList();
                obj.hotelsearchResponses = hotelfilter;
                obj.m_SearchHotel = result.m_SearchHotel;
              
                List<StarRating> slist = new List<StarRating>();
                var ratingfilter = hotelfilter.GroupBy(p => p.starRating).OrderByDescending(p => p.Key).ToList();
                foreach (var item in ratingfilter)
                {
                    System.String s = Convert.ToString(item.Key);
                    int decimalPosition = s.IndexOf('.');
                    if (decimalPosition == -1)
                    {
                        StarRating ss = new StarRating()
                        {
                            Rating = Convert.ToString(s)
                        };
                        slist.Add(ss);
                    }
                }
                obj.starRatings = slist;
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/LiveHotel/Roomlist.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/LiveHotel/Roomlist.cshtml", obj);
                }
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }
        }
    }
}
