using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LiveHolidayapp.Controllers
{
    public class HomeController : Controller
    {
        CompanyDetail _companyDetail;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string Theme = "";
        M_Company obj = new M_Company();
        R_Hotel _Hotel = new R_Hotel();
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {

            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            obj = this._companyDetail.GetCompany();
            Theme = obj.Theme;
        }

        public IActionResult Index()
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("CompanyId")) == 4306)
            {
                return RedirectToAction("Dreamdays", "Home");
            }
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/index.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/index.cshtml");
            }

        }
        public ActionResult Dreamdays()
        {
            return View("~/Views/Shared/Dreamdaysindex.cshtml");
        }


        public IActionResult Privacy()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/Privacy.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/Privacy.cshtml");
            }
        }

        public IActionResult About()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/About.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/About.cshtml");
            }
        }

        public IActionResult Termscondition()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/Termscondition.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/Termscondition.cshtml");
            }
        }

        public IActionResult Contactus()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/Contactus.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/Contactus.cshtml");
            }
        }

        public IActionResult BookHotel()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                if (HttpContext.Session.GetString("IsHotel") == "R")
                {
                    return RedirectToAction("SearchHotel", "LiveHotel");
                }
                if (HttpContext.Session.GetString("IsHotel") == "RT")
                {
                    return RedirectToAction("SearchHotel", "HotelMerge");
                }
                else
                {
                    return RedirectToAction("HotelSearch", "Hotel");
                }
            }
            else
            {
                //if (HttpContext.Session.GetString("IsFrontLogin") == "N")
                //{
                //    return RedirectToAction(nameof(HomeController.Index), "Home");
                //}
                //else
                //{
                //    string returnUrl = Url.Action("SearchHotel", "LiveHotel")!;
                //    return RedirectToAction("Login", "Account", new { returnUrl });
                //}
                if (HttpContext.Session.GetString("IsHotel") == "R")
                {
                    string returnUrl = Url.Action("SearchHotel", "LiveHotel")!;
                    return RedirectToAction("Login", "Account", new { returnUrl });
                }
                if (HttpContext.Session.GetString("IsHotel") == "RT")
                {
                    string returnUrl = Url.Action("SearchHotel", "HotelMerge")!;
                    return RedirectToAction("Login", "Account", new { returnUrl });
                }
                else
                {
                    string returnUrl = Url.Action("HotelSearch", "Hotel")!;
                    return RedirectToAction("Login", "Account", new { returnUrl });
                }
            }

        }
        public IActionResult Destinations()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/Destinations.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/Destinations.cshtml");
            }
        }
        public IActionResult Refund()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Home/Refund.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/Refund.cshtml");
            }
        }

        public IActionResult Redemption()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                BookHotelReport obj = new BookHotelReport();
                HotelReq hotelReq = new HotelReq();
                hotelReq.Formno = Convert.ToInt32(HttpContext.Session.GetString("FormNo"));
                //hotelReq.Formno = 22859;
                var reportoutput = _Hotel.GetHotelreport(hotelReq, Convert.ToString(HttpContext.Session.GetString("Authnekot"))!);
                var output = JsonConvert.DeserializeObject<CommonResponse<List<M_Hotelreport>>>(reportoutput);
                if (output != null && output.Code == 200)
                {
                    obj.Hotelreports = output.Data;
                }
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/Home/Redemption.cshtml",obj);
                }
                else
                {
                    return View("~/Views/Theme/Home/Redemption.cshtml",obj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
