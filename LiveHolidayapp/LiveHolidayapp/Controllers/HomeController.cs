using LiveHolidayapp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LiveHolidayapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CompanyDetail cmd = new CompanyDetail();
        private readonly string Theme = "Theme";
        public HomeController(ILogger<HomeController> logger)
        {

            _logger = logger;
            //M_Company obj = cmd.GetCompany();
            //Theme = obj.Theme;
        }

        public IActionResult Index()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/"+Theme+ "/Home/index.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Home/index.cshtml");
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
