using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using X.PagedList;
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
        General gen = new General();
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

        public IActionResult Roomlist(int? pageNo)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                int pageIndex = 1;
                pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                M_Hotel obj = new M_Hotel();

                var PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
                var PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                var hotelfilter = result.hotelsearchResponses.Where(p => Convert.ToDecimal(p.price) >= PriceRangeStart && Convert.ToDecimal(p.price) <= PriceRangeEnd).ToList();
                obj.hotelsearchResponses = hotelfilter;
                var pagining = hotelfilter.ToPagedList((int)pageIndex, 12);
                obj.Hotelpaging = pagining;

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

        [HttpPost]
        public IActionResult FilterHotelName(string name)
        {
            M_Hotel obj = new M_Hotel();
            var PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
            var PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
            var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
            var hotelfilter = result.hotelsearchResponses.Where(p => Convert.ToDecimal(p.price) >= PriceRangeStart && Convert.ToDecimal(p.price) <= PriceRangeEnd).ToList();
            if (name == null || name == "")
            {
                var hotelname = hotelfilter.ToList();
                obj.hotelsearchResponses = hotelname;
            }
            else
            {
                var hotelname = hotelfilter.Where(p => Regex.IsMatch(p.hotelName, name, RegexOptions.IgnoreCase)).ToList();
                obj.hotelsearchResponses = hotelname;
            }

            var view = "";
            if (Theme != null && Theme != "")
            {
                view = "~/Views/" + Theme + "/LiveHotel/_FilterHotenamePartial.cshtml";
            }
            else
            {
                view = "~/Views/Theme/LiveHotel/_FilterHotenamePartial.cshtml";
            }
            return PartialView(view, obj);
        }

        public IActionResult ApplyFilter(string rating, string hotelname, int? page)
        {
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            M_Hotel obj = new M_Hotel();
            var PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
            var PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
            var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
            var hotelfilter = result.hotelsearchResponses.Where(p => Convert.ToDecimal(p.price) >= PriceRangeStart && Convert.ToDecimal(p.price) <= PriceRangeEnd).ToList();
            if (!string.IsNullOrEmpty(rating) && string.IsNullOrEmpty(hotelname))
            {
                var strStarRating = rating.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => strStarRating.Contains(p.starRating)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, 12);
                obj.Hotelpaging = pagining;
            }
            else if (string.IsNullOrEmpty(rating) && !string.IsNullOrEmpty(hotelname))
            {
                var hotnam = hotelname.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => hotnam.Contains(p.hotelName)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, 12);
                obj.Hotelpaging = pagining;
            }
            else if (!string.IsNullOrEmpty(rating) && !string.IsNullOrEmpty(hotelname))
            {
                var strStarRating = rating.Split(',').ToList();
                var hotnam = hotelname.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => strStarRating.Contains(p.starRating) && hotnam.Contains(p.hotelName)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, 12);
                obj.Hotelpaging = pagining;
            }
            else
            {
                //obj.hotelsearchResponses = hotelfilter;
                var pagining = hotelfilter.ToPagedList((int)pageIndex, 12);
                obj.Hotelpaging = pagining;
            }
            var view = "";
            if (Theme != null && Theme != "")
            {
                view = "~/Views/" + Theme + "/LiveHotel/_FilterHoteDataPartial.cshtml";
            }
            else
            {
                view = "~/Views/Theme/LiveHotel/_FilterHoteDataPartial.cshtml";
            }
            return PartialView(view, obj);
        }

        public IActionResult RoomDetails(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                ViewBag.Hotelid = id;
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                var sortdata = result.hotelsearchResponses.Where(p => Convert.ToInt32(p.hotelResults_ID) == id).ToList();
                string response = string.Empty;
                response = _Hotel.PropertyDetail(id);
                PropertyDetailRoot data = new PropertyDetailRoot();
                M_Hotel obj = new M_Hotel();
                if (response != "")
                {
                    data = JsonConvert.DeserializeObject<PropertyDetailRoot>(response);
                    if (data.success != false)
                    {
                        obj.Amenities = data.propertyDetail.Amenities;
                        obj.images = data.propertyDetail.images;
                    }
                }
                obj.hotelsearchResponses = sortdata;
                obj.m_SearchHotel = result.m_SearchHotel;
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/LiveHotel/RoomDetails.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/LiveHotel/RoomDetails.cshtml", obj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult BookHotal(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                ViewBag.Hotelid = id;
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                M_Hotel obj = new M_Hotel();
                obj.m_SearchHotel = result.m_SearchHotel;
                if (HttpContext.Session.GetString("CompanyId") == "4844")
                {
                    //check isholiday
                    var res = _Hotel.CheckPackage(HttpContext.Session.GetString("UserName"), HttpContext.Session.GetString("password"));
                    DataSet ds = new DataSet();
                    ds = gen.convertJsonStringToDataSet(res);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        HttpContext.Session.SetString("isholiday", Convert.ToString(ds.Tables[0].Rows[0]["isholiday"]));
                    }
                }
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/LiveHotel/BookHotal.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/LiveHotel/BookHotal.cshtml", obj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult HotelBook(M_HotelBook HotelBookreq)
        {
            string msg = string.Empty;
            string err = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                if (HttpContext.Session.GetString("CompanyId") == "4844")
                {
                    if (HttpContext.Session.GetString("isholiday") == "N")
                    {
                        msg = "Please purchase package.";
                        err = "1";
                        return Json(new { msg, err });
                    }
                }
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                var sortdata = result.hotelsearchResponses.Where(p => Convert.ToInt32(p.hotelResults_ID) == Convert.ToInt32(HotelBookreq.hotelCode)).ToList();
                HotelBookreq.userName = Convert.ToString(HttpContext.Session.GetString("Name"));
                HotelBookreq.formNo = Convert.ToString(HttpContext.Session.GetString("FormNo"));
                HotelBookreq.companyId = Convert.ToString(HttpContext.Session.GetString("CompanyId"));
                HotelBookreq.adultCount = result.m_SearchHotel.ddlAdult;
                HotelBookreq.childCount = result.m_SearchHotel.ddlChild;
                HotelBookreq.cityName = result.m_SearchHotel.City;
                HotelBookreq.noOfRoom = "1";
                HotelBookreq.checkInDate = result.m_SearchHotel.txtHotelCheckIn;
                HotelBookreq.checkOutDate = result.m_SearchHotel.txtHotelCheckOut;
                HotelBookreq.orderId = "0";
                HotelBookreq.bookingAmount = Convert.ToString(Math.Round(Convert.ToDecimal(sortdata[0].price), 2));
                HotelBookreq.hotelName = Convert.ToString(sortdata[0].hotelName);
                HotelBookreq.hotelCode = Convert.ToString(sortdata[0].hotelCode);
                HotelBookreq.dob = "22/04/2019";
                var Response = _Hotel.HotelBook(HotelBookreq, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var output = JsonConvert.DeserializeObject<CommonResponse<HotelBookresponse>>(Response);
                if (output != null)
                {
                    if (output.Code == 200 && output.Message == "Hotel Book Successfully" && output.Data.bookStatus == "P")
                    {
                        msg = "Hotel Book Successfully";
                        err = "0";
                        HttpContext.Session.SetComplexData("BookResponse", output.Data);
                    }
                    else
                    {
                        msg = "Your Booking failed.";
                        err = "1";
                    }
                }
                else
                {
                    msg = "Your Booking failed.";
                    err = "1";
                }

            }
            return Json(new { msg, err });
        }

        public IActionResult BookingThankyou()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                HotelBookresponse obj = new HotelBookresponse();
                obj = HttpContext.Session.GetComplexData<HotelBookresponse>("BookResponse");
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/LiveHotel/BookingThankyou.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/LiveHotel/BookingThankyou.cshtml", obj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
    }
}
