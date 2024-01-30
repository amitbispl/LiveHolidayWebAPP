using LiveHolidayapp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LiveHolidayapp.Controllers
{
    public class HomeController : Controller
    {
        CompanyDetail _companyDetail;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string Theme = "Theme";
        M_Company obj = new M_Company();
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {

            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            obj = this._companyDetail.GetCompany();
            //Theme = obj.Theme;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Authnekot")))
            {
                if (Theme != null && Theme != "")
                {
                    return View("~/Views/" + Theme + "/Home/index.cshtml");
                }
                else
                {
                    return View("~/Views/Theme/Home/index.cshtml");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
