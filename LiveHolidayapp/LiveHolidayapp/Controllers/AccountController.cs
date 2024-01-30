using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LiveHolidayapp.Controllers
{
    public class AccountController : Controller
    {
         CompanyDetail _companyDetail;
        private readonly string Theme = "Theme";
        M_Company obj = new M_Company();
        private IHttpContextAccessor _httpContextAccessor;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            obj = this._companyDetail.GetCompany();
        }
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
                var response = await rr.UserLogin(req);
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
                //HttpContext.Session.SetString("OrderId", response.OrderId);
                return RedirectToAction("Index", "Home");
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


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }




    }
}
