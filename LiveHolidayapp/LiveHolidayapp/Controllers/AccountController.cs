using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LiveHolidayapp.Controllers
{
    public class AccountController : Controller
    {
        CompanyDetail cmd = new CompanyDetail();
        private readonly string Theme = "Theme";
        public IActionResult Login()
        {
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Account/Login.cshtml");
            }
            else
            {
                return View("~/Views/Theme/Account/Login.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(M_Login obj)
        {
            if (ModelState.IsValid)
            {
                Loginreq req = new Loginreq();
                req.companyId = 1;
                req.password = obj.Password;
                req.userName = obj.Username;
                R_Login rr = new R_Login();
               var response=await rr.UserLogin(req);
            }
            if (Theme != null && Theme != "")
            {
                return View("~/Views/" + Theme + "/Account/Login.cshtml", obj);
            }
            else
            {
                return View("~/Views/Theme/Account/Login.cshtml", obj);
            }

        }

       

    }
}
