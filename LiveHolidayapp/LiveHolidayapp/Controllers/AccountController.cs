using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LiveHolidayapp.Controllers
{
    public class AccountController : Controller
    {
        CompanyDetail _companyDetail;
        private readonly string Theme = "";
        M_Company Cobj = new M_Company();
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private string companyId = "";
        public AccountController(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            Cobj = this._companyDetail.GetCompany();
            _config = config;
            companyId =Convert.ToString(Cobj.CompanyId);
            Theme = Cobj.Theme;
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
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
        public async Task<IActionResult> Login(M_Login obj, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                Loginreq req = new Loginreq();
                req.companyId = Convert.ToInt32(companyId);
                req.password = obj.Password;
                req.userName = obj.Username;
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
                    //HttpContext.Session.SetString("OrderId", response.OrderId);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                
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
            return RedirectToAction("Index", "Home");
        }




    }
}
