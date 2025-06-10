using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using X.PagedList;

namespace LiveHolidayapp.Controllers
{
    public class HotelMergeController : Controller
    {
        CompanyDetail _companyDetail;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string Theme = "";
        M_Company obj = new M_Company();
        R_Hotel _Hotel = new R_Hotel();
        private static string LastCountryname = "";
        General gen = new General();
        public HotelMergeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
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
                if (HttpContext.Session.GetString("isRedeem") == "True")
                {
                    return RedirectToAction("Redemption", "Home");
                }

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
                                    DateTime futureDate = DateTime.ParseExact(checkoutdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
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
                    catch(Exception ex)
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
                    return View("~/Views/" + Theme + "/HotelMerge/SearchHotel.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/HotelMerge/SearchHotel.cshtml", obj);
                }
            }
            else
            {
                string returnUrl = Url.Action("SearchHotel", "HotelMerge")!;
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
        }
        [HttpPost]
        public IActionResult GetCitylist(string Cityname, string countrycode)
        {
            List<Citylist> filterdata = new List<Citylist>();
            List<Citylist> data = HttpContext.Session.GetComplexData<List<Citylist>>("citylist");
            if (LastCountryname == countrycode && countrycode == data[0].countryCode)
            {

                if (Cityname != null)
                {
                    filterdata = data.Where(p => Regex.IsMatch(p.cityName, Cityname, RegexOptions.IgnoreCase)).ToList();
                }
                else
                {
                    filterdata = data;
                }
            }
            else
            {
                Citylistreq cityreq = new Citylistreq();
                cityreq.cityName = Cityname == null ? "" : Cityname;
                cityreq.countryCode = countrycode;
                cityreq.registerId = Convert.ToInt32(HttpContext.Session.GetString("registerId"));
                List<Citylist> citylist = new List<Citylist>();
                citylist = _Hotel.GetCitylist(cityreq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                HttpContext.Session.SetComplexData("citylist", citylist);
                LastCountryname = countrycode;
                if (Cityname != null)
                {
                    filterdata = citylist.Where(p => Regex.IsMatch(p.cityName, Cityname, RegexOptions.IgnoreCase)).ToList();
                }
                else
                {
                    filterdata = citylist;
                }
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
                //var response = _Hotel.HotelMergesearchResponse(Hotelreq, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var response = _Hotel.HotelMergesearchResponseWithCaching(Hotelreq, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
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
                        return View("~/Views/" + Theme + "/HotelMerge/RoomlistGrid.cshtml", obj);
                    }
                    else
                    {
                        return View("~/Views/Theme/HotelMerge/RoomlistGrid.cshtml", obj);
                    }
                }
                else
                {
                    if (Theme != null && Theme != "")
                    {
                        return View("~/Views/" + Theme + "/HotelMerge/Roomlist.cshtml", obj);
                    }
                    else
                    {
                        return View("~/Views/Theme/HotelMerge/Roomlist.cshtml", obj);
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
                view = "~/Views/" + Theme + "/HotelMerge/_FilterHotenamePartial.cshtml";
            }
            else
            {
                view = "~/Views/Theme/HotelMerge/_FilterHotenamePartial.cshtml";
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
                    view = "~/Views/" + Theme + "/HotelMerge/_FilterHoteDataGridPartial.cshtml";
                }
                else
                {
                    view = "~/Views/Theme/HotelMerge/_FilterHoteDataGridPartial.cshtml";
                }
            }
            else
            {
                if (Theme != null && Theme != "")
                {
                    view = "~/Views/" + Theme + "/HotelMerge/_FilterHoteDataPartial.cshtml";
                }
                else
                {
                    view = "~/Views/Theme/HotelMerge/_FilterHoteDataPartial.cshtml";
                }
            }

            return PartialView(view, obj);
        }

        public IActionResult RoomDetails(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                ViewBag.Hotelid = id;
                M_Hotel obj = new M_Hotel();
                var result = HttpContext.Session.GetComplexData<M_Hotel>("hotelsearchResponses");
                var sortdata = result.hotelsearchResponses.Where(p => Convert.ToInt32(p.hotelResults_ID) == id).ToList();
                HotelDetailreq detailreq = new HotelDetailreq();
                detailreq.Hotelcodes = sortdata[0].hotelCodes;
                string response = string.Empty;
                response = _Hotel.HotelMergeDetail(detailreq, Convert.ToString(HttpContext.Session.GetString("Authnekot")));
                var output = JsonConvert.DeserializeObject<CommonResponse<HoteldetailCommonres>>(response);
                response = _Hotel.PropertyDetail(id);
                if (output != null)
                {
                    if (output.Message == "Success")
                    {
                        obj.hoteldetailCommonres = output.Data;
                    }
                }
                obj.hotelsearchResponses = sortdata;
                obj.m_SearchHotel = result.m_SearchHotel;
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/HotelMerge/RoomDetails.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Theme/HotelMerge/RoomDetails.cshtml", obj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
