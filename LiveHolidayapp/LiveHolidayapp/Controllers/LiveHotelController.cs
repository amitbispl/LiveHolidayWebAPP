using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using X.PagedList;
using static System.Net.WebRequestMethods;
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
                if (HttpContext.Session.GetString("Retopup") == "True")
                {
                    try
                    {
                        HotelReq hotelReq = new HotelReq();
                        hotelReq.Formno = Convert.ToInt32(HttpContext.Session.GetString("FormNo"));
                        var reportoutput = _Hotel.GetHotelreport(hotelReq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                        var output = JsonConvert.DeserializeObject<CommonResponse<List<M_Hotelreport>>>(reportoutput);
                        if (output != null && output.Code == 200)
                        {
                            if (output.Data.Count() > 0)
                            {
                                List<M_Hotelreport> hrs = output.Data;
                                if (hrs != null && hrs.Count > 0)
                                {
                                    var checkoutdate = Convert.ToString(hrs[0].CheckOutDate);
                                    // Define two dates
                                    DateTime today = DateTime.Today;
                                    DateTime futureDate = DateTime.ParseExact(checkoutdate, "dd-MM-yyyy HH:mm:ss", null);
                                    //DateTime futureDate = Convert.ToDateTime(checkoutdate).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    //DateTime futureDate = new DateTime(2024, 8, 18);
                                    // Calculate the difference between the two dates
                                    TimeSpan difference = futureDate - today;
                                    // Get the difference in days
                                    int daysDifference = difference.Days;
                                    daysDifference = daysDifference + 2;
                                    _httpContextAccessor.HttpContext.Session.SetString("StartAfterday", Convert.ToString(daysDifference)!);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (Convert.ToString(HttpContext.Session.GetString("OrderId")) != "0" && Convert.ToBoolean(HttpContext.Session.GetString("IsManualSelectPackage")) == true)
                        {
                            var reportoutput = _Hotel.GetpackageList(Convert.ToString(HttpContext.Session.GetString("FormNo")), Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                            var output = JsonConvert.DeserializeObject<CommonResponse<List<M_PackageTypes>>>(reportoutput);
                            if (output != null && output.Code == 200)
                            {
                                obj.PackageTypesList = output.Data;
                            }
                        }
                    }
                    catch
                    {

                    }
                }


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
                if (Convert.ToString(HttpContext.Session.GetString("OrderId")) != "0" && Convert.ToBoolean(HttpContext.Session.GetString("IsManualSelectPackage")) == true)
                {
                    HttpContext.Session.SetString("OrderId", Convert.ToString(Hotelreq.Orderid));
                    HttpContext.Session.SetString("IDWiseDayAfter", Convert.ToString(Hotelreq.IDWiseDayAfter));
                }
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
                        if (list.Count != 0)
                        {
                            obj.hotelsearchResponses = list;
                            HttpContext.Session.SetComplexData("hotelsearchResponses", obj);
                            msg = "Success";
                        }
                        else
                        {
                            msg = "No Hotel Found";
                        }
                    }
                    else
                    {
                        msg = output.Message;
                    }

                }
                else
                {
                    msg = "No Hotel Found";
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
                int pagesize = 16;
                pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
                M_Hotel obj = new M_Hotel();

                decimal PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
                decimal PriceRangeEnd = 0;
                if (HttpContext.Session.GetString("Retopup") == "True")
                {
                    var tempPriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                    int dayafter = 0;
                    if (Convert.ToString(HttpContext.Session.GetString("OrderId")) == "0")
                    {
                        dayafter = Convert.ToInt32(HttpContext.Session.GetString("DayAfter"));
                    }
                    else
                    {
                        dayafter = Convert.ToInt32(HttpContext.Session.GetString("IDWiseDayAfter"));
                    }
                    PriceRangeEnd = tempPriceRangeEnd * dayafter;

                }
                else
                {
                    PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                }
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");


                var hotelfilter = result.hotelsearchResponses.Where(p => p.price >= PriceRangeStart && p.price <= PriceRangeEnd).ToList();

                obj.hotelsearchResponses = hotelfilter;
                if (Convert.ToString(HttpContext.Session.GetString("HotelListTheme")) == "G")
                {
                    pagesize = 18;
                }
                var pagining = hotelfilter.ToPagedList((int)pageIndex, pagesize);
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
                if (Convert.ToString(HttpContext.Session.GetString("HotelListTheme")) == "G")
                {
                    if (Theme != null && Theme != "")
                    {
                        return View("~/Views/" + Theme + "/LiveHotel/RoomlistGrid.cshtml", obj);
                    }
                    else
                    {
                        return View("~/Views/Theme/LiveHotel/RoomlistGrid.cshtml", obj);
                    }
                }
                else
                {
                    if (Theme != null && Theme != "")
                    {
                        return View("~/Views/" + Theme + "/LiveHotel/Roomlist.cshtml", obj);
                    }
                    else
                    {
                        return View("~/Views/Theme/LiveHotel/Roomlist.cshtml", obj);
                    }
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
            decimal PriceRangeEnd = 0;
            if (HttpContext.Session.GetString("Retopup") == "True")
            {
                var tempPriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                int dayafter = 0;
                if (Convert.ToString(HttpContext.Session.GetString("OrderId")) == "0")
                {
                    dayafter = Convert.ToInt32(HttpContext.Session.GetString("DayAfter"));
                }
                else
                {
                    dayafter = Convert.ToInt32(HttpContext.Session.GetString("IDWiseDayAfter"));
                }
                PriceRangeEnd = tempPriceRangeEnd * dayafter;
            }
            else
            {
                PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
            }
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
            int pagesize = 16;
            if (Convert.ToString(HttpContext.Session.GetString("HotelListTheme")) == "G")
            {
                pagesize = 18;
            }
            M_Hotel obj = new M_Hotel();
            var PriceRangeStart = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeStart"));
            decimal PriceRangeEnd = 0;
            if (HttpContext.Session.GetString("Retopup") == "True")
            {
                var tempPriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
                int dayafter = 0;
                if (Convert.ToString(HttpContext.Session.GetString("OrderId")) == "0")
                {
                    dayafter = Convert.ToInt32(HttpContext.Session.GetString("DayAfter"));
                }
                else
                {
                    dayafter = Convert.ToInt32(HttpContext.Session.GetString("IDWiseDayAfter"));
                }
                PriceRangeEnd = tempPriceRangeEnd * dayafter;
            }
            else
            {
                PriceRangeEnd = Convert.ToDecimal(HttpContext.Session.GetString("PriceRangeEnd"));
            }
            var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
            var hotelfilter = result.hotelsearchResponses.Where(p => Convert.ToDecimal(p.price) >= PriceRangeStart && Convert.ToDecimal(p.price) <= PriceRangeEnd).ToList();
            if (!string.IsNullOrEmpty(rating) && string.IsNullOrEmpty(hotelname))
            {
                var strStarRating = rating.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => strStarRating.Contains(p.starRating)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, pagesize);
                obj.Hotelpaging = pagining;
            }
            else if (string.IsNullOrEmpty(rating) && !string.IsNullOrEmpty(hotelname))
            {
                var hotnam = hotelname.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => hotnam.Contains(p.hotelName)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, pagesize);
                obj.Hotelpaging = pagining;
            }
            else if (!string.IsNullOrEmpty(rating) && !string.IsNullOrEmpty(hotelname))
            {
                var strStarRating = rating.Split(',').ToList();
                var hotnam = hotelname.Split(',').ToList();
                var cusfilter = hotelfilter.Where(p => strStarRating.Contains(p.starRating) && hotnam.Contains(p.hotelName)).ToList();
                var pagining = cusfilter.ToPagedList((int)pageIndex, pagesize);
                obj.Hotelpaging = pagining;
            }
            else
            {
                //obj.hotelsearchResponses = hotelfilter;
                var pagining = hotelfilter.ToPagedList((int)pageIndex, pagesize);
                obj.Hotelpaging = pagining;
            }
            obj.m_SearchHotel = result.m_SearchHotel;
            var view = "";
            if (Convert.ToString(HttpContext.Session.GetString("HotelListTheme")) == "G")
            {
                if (Theme != null && Theme != "")
                {
                    view = "~/Views/" + Theme + "/LiveHotel/_FilterHoteDataGridPartial.cshtml";
                }
                else
                {
                    view = "~/Views/Theme/LiveHotel/_FilterHoteDataGridPartial.cshtml";
                }
            }
            else
            {
                if (Theme != null && Theme != "")
                {
                    view = "~/Views/" + Theme + "/LiveHotel/_FilterHoteDataPartial.cshtml";
                }
                else
                {
                    view = "~/Views/Theme/LiveHotel/_FilterHoteDataPartial.cshtml";
                }
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
                try
                {
                    if (response != "")
                    {
                        data = JsonConvert.DeserializeObject<PropertyDetailRoot>(response);
                        if (data.success != false)
                        {
                            obj.Amenities = data.propertyDetail.Amenities;
                            if (obj.Amenities.Count() > 0)
                            {
                                var iconMappings = new Dictionary<string, string>
                                  {
                                      { "Safe-deposit box at front desk", "fa fa-lock" },
                                      { "Supervised childcare/activities", "fa fa-child" },
                                      { "Breakfast available (surcharge)", "fa fa-coffee" },
                                      { "Designated smoking areas", "fa fa-smoking" },
                                      { "Accessible bathroom", "fa fa-bath" },
                                      { "Free self-parking", "fa fa-car" },
                                      { "Free newspapers in lobby", "fa-solid fa-newspaper" },
                                      { "Restaurant - 1", "fa fa-cutlery" },
                                      { "Free WiFi", "fa fa-wifi" },
                                      { "Luggage storage", "fas fa-luggage-cart" },
                                      { "Picnic area", "fa-regular fa-circle-check" },
                                      { "Express check-in", "fa fa-check" },
                                      { "Couples/private dining", "fa fa-cutlery" },
                                      { "Children's toys", "fa-solid fa-baseball-bat-ball" },
                                      { "Elevator", "fa-solid fa-elevator" },
                                      { "24-hour front desk", "fa fa-phone" },
                                      { "Free breakfast", "fa-solid fa-mug-saucer" },
                                      { "Free self parking", "fa-solid fa-square-parking" },
                                      { "Smoke-free property", "fas fa-smoking-ban" },
                                      { "Meeting rooms", "fa-solid fa-handshake" },
                                      { "Number of bars/lounges - 2", "fas fa-glass-martini" },
                                      { "Coffee shop or cafÃ©", "fas fa-coffee" },
                                      { "Free wired Internet", "fa-solid fa-globe" },
                                      { "Wheelchair accessible path of travel", "fas fa-wheelchair" },
                                      { "Television in common areasl", "fas fa-tv" },
                                      { "Outdoor pool", "fas fa-swimming-pool" },
                                      { "Garden", "fa-solid fa-tree" },
                                      { "Restaurant", "fa fa-cutlery" },
                                      { "Bar/lounge", "fas fa-glass-martini-alt" },
                                      { "One meeting room", "fa-solid fa-handshake" },
                                      { "Airport transportation (surcharge)", "fa fa-plane" },
                                  };
                             
                                var roomServices = obj.Amenities.ToDictionary(
                                      s => s,
                                      s => iconMappings.ContainsKey(s) ? iconMappings[s] : "fa-regular fa-circle-check" // Default icon
                                      );
                                obj.Amenitiesdisctionary = roomServices;
                            }
                            
                            obj.images = data.propertyDetail.images;
                        }
                    }
                }
                catch
                {

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
                if (HttpContext.Session.GetString("Retopup") == "True")
                {
                    //check isholiday
                    try
                    {
                        if (HttpContext.Session.GetString("OrderId") != "0")
                        {
                            HttpContext.Session.SetString("isholiday", "Y");
                        }
                        else
                        {
                            HttpContext.Session.SetString("isholiday", "N");
                        }
                    }
                    catch
                    {

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
        public IActionResult SendOtp(string MobileNo)
        {
            try
            {
                if (HttpContext.Session.GetString("Retopup") == "False" && HttpContext.Session.GetString("isRedeem") == "True")
                {
                    return Json(new { Code = 300, msg = "Already redeemed this service", otpid = 0 });
                }
                OTPRequest req = new OTPRequest();
                req.MobileNo = MobileNo;
                req.FormNo = Convert.ToString(HttpContext.Session.GetString("FormNo"));
                req.CompanyId = Convert.ToInt32(HttpContext.Session.GetString("CompanyId"));
                req.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
                var Response = _Hotel.SendOTP(req, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var output = JsonConvert.DeserializeObject<CommonResponse<OTPREsponse>>(Response);
                if (output != null)
                {
                    if (output.Code == 200)
                    {
                        return Json(new { Code = 200, msg = output.Message, otpid = output.Data.ID });
                    }
                    else
                    {
                        return Json(new { Code = 300, msg = output.Message, otpid = 0 });
                    }
                }
                else
                {
                    return Json(new { Code = 300, msg = "OTP not send", otpid = 0 });
                }
            }
            catch
            {
                return Json(new { Code = 300, msg = "Something went wrong", otpid = 0 });
            }
        }

        [HttpPost]
        public IActionResult ValidateOTP(string Otpid, string Otpval)
        {
            try
            {
                ValidOTPReq req = new ValidOTPReq();
                req.ID = Convert.ToInt32(Otpid);
                req.OTP = Otpval;
                var Response = _Hotel.ValidateOTP(req, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var output = JsonConvert.DeserializeObject<CommonResponse<string>>(Response);
                if (output != null)
                {
                    if (output.Code == 200)
                    {
                        return Json(new { Code = 200, msg = output.Message });
                    }
                    else
                    {
                        return Json(new { Code = 100, msg = output.Message });
                    }
                }
                else
                {
                    return Json(new { Code = 100, msg = "OTP not matched" });
                }
            }
            catch
            {
                return Json(new { Code = 100, msg = "OTP not matched" });
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
                if (HttpContext.Session.GetString("IsHotel") == "T")
                {
                    //this code for tbo hotel
                    var inputDate = result.m_SearchHotel.txtHotelCheckIn.Split('-');
                    string formattedDate = inputDate[2] + "/" + inputDate[1] + "/" + inputDate[0];
                    HotelBookreq.checkInDate = formattedDate;

                    var inputDate1 = result.m_SearchHotel.txtHotelCheckOut.Split('-');
                    formattedDate = inputDate1[2] + "/" + inputDate1[1] + "/" + inputDate1[0];
                    HotelBookreq.checkOutDate = formattedDate;
                }
                else
                {
                    HotelBookreq.checkInDate = result.m_SearchHotel.txtHotelCheckIn;
                    HotelBookreq.checkOutDate = result.m_SearchHotel.txtHotelCheckOut;
                }
                HotelBookreq.orderId = Convert.ToString(HttpContext.Session.GetString("OrderId"));
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

        public async Task<IActionResult> BookingThankyou()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                HotelBookresponse obj = new HotelBookresponse();
                obj = HttpContext.Session.GetComplexData<HotelBookresponse>("BookResponse");
                try
                {
                    Loginreq req = new Loginreq();
                    req.companyId = Convert.ToInt32(HttpContext.Session.GetString("CompanyId"));
                    req.password = HttpContext.Session.GetString("password");
                    req.userName = HttpContext.Session.GetString("UserName");
                    R_Login rr = new R_Login();
                    var response = await rr.UserLogin(req);
                    if (response != null)
                    {
                        HttpContext.Session.SetString("Authnekot", response.tokenString);
                        HttpContext.Session.SetString("FormNo", Convert.ToString(response.formNo));
                        HttpContext.Session.SetString("RegisterId", Convert.ToString(response.id));
                        HttpContext.Session.SetString("KitID", Convert.ToString(response.kitId));
                        HttpContext.Session.SetString("Name", response.name);
                        HttpContext.Session.SetString("isRedeem", Convert.ToString(response.isRedeem));
                        HttpContext.Session.SetString("EmailID", Convert.ToString(response.email));
                        HttpContext.Session.SetString("doj", Convert.ToString(response.doj));
                        HttpContext.Session.SetString("Status", "OK");
                        HttpContext.Session.SetString("MobileNo", response.mobileNo);
                        HttpContext.Session.SetString("UserName", response.userName);
                        HttpContext.Session.SetString("registerId", Convert.ToString(response.id));
                        HttpContext.Session.SetString("OrderId", Convert.ToString(response.orderId));
                        HttpContext.Session.SetString("IDWiseDayAfter", Convert.ToString(response.IDWiseDayAfter));
                    }
                    else
                    {
                        ViewBag.message = "Please enter valid username and password";
                        if (HttpContext.Session.GetString("Retopup") == "False")
                        {
                            HttpContext.Session.SetString("isRedeem", Convert.ToString("True"));
                        }
                    }

                }
                catch
                {

                }
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
