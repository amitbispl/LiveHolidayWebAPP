using LiveHolidayapp.Models;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System;

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
        General clsgen = new General();
        public AccountController(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            this._companyDetail = new CompanyDetail(_httpContextAccessor);
            Cobj = this._companyDetail.GetCompany();
            _config = config;
            companyId = Convert.ToString(Cobj.CompanyId);
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
            try
            {
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
                        HttpContext.Session.SetString("password", Convert.ToString(obj.Password));
                        HttpContext.Session.SetString("OrderId", Convert.ToString(response.orderId));
                        HttpContext.Session.SetString("IDWiseDayAfter", Convert.ToString(response.IDWiseDayAfter));
                        if (response.isRedeem == true)
                        {
                            return RedirectToAction("Redemption", "Home");
                        }
                        if (Convert.ToInt32(HttpContext.Session.GetString("CompanyId")) == 4306)
                        {
                            if (HttpContext.Session.GetString("IsHotel") == "R")
                            {
                                return RedirectToAction("SearchHotel", "LiveHotel");
                            }
                            else
                            {
                                return RedirectToAction("HotelSearch", "Hotel");
                            }
                        }
                        else if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

                    }
                    else
                    {
                        ViewBag.message = "Please enter valid username and password";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = "Please enter valid username and password";
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

        public async Task<IActionResult> DirectLogin(string URL)
        {
            try
            {
                string action = string.Empty;
                string userName = string.Empty;
                string token = string.Empty;
                string apiURL = string.Empty;
                string voucherNo = string.Empty;
                string txtnData = string.Empty;
                string formNo = string.Empty;
                string pcode = string.Empty;
                General.Base64Decode(URL);
                string data = General.Base64Decode(URL);
                // Split string on spaces (this will separate all the words).
                string[] words = data.Split('&');
                string user = string.Empty;
                string password = string.Empty;
                string act = string.Empty;
                string tkn = string.Empty;
                string cpn = string.Empty;
                string pc = string.Empty;
                user = words[0].ToString();
                password = words[1].ToString();
                act = words[2].ToString();
                tkn = words[3].ToString();
                if (words.Count() == 5)
                {
                    pc = Convert.ToString(words[4]);
                }
                //cpn = words[4].ToString();

                string[] u1 = user.Split('=');
                string[] p1 = password.Split('=');
                string[] t1 = act.Split('=');
                string[] a1 = tkn.Split('=');
                //string[] c1 = cpn.Split('=');
                if (!string.IsNullOrEmpty(pc))
                {
                    string[] d1 = pc.Split('=');
                    pcode = d1[1].ToString();
                }
                userName = u1[1].ToString();
                password = p1[1].ToString();
                action = t1[1].ToString();
                token = a1[1].ToString();
                if (Convert.ToString(HttpContext.Session.GetString("CmpToken")) == token)
                {
                    M_Login obj = new M_Login();
                    Loginreq req = new Loginreq();
                    req.companyId = Convert.ToInt32(companyId);
                    req.password = password;
                    req.userName = userName;
                    R_Login rr = new R_Login();
                    var response = await rr.UserLogin(req);
                    if (response != null)
                    {
                        if (response.isRedeem == true)
                        {
                            ViewBag.message = "Already redeemed this service";
                            return RedirectToAction("Index", "Home");
                        }
                        else
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
                            HttpContext.Session.SetString("OrderId", Convert.ToString(response.orderId));
                            HttpContext.Session.SetString("IDWiseDayAfter", Convert.ToString(response.IDWiseDayAfter));
                            if (Convert.ToInt32(HttpContext.Session.GetString("CompanyId")) == 4306)
                            {
                                if (HttpContext.Session.GetString("IsHotel") == "R")
                                {
                                    return RedirectToAction("SearchHotel", "LiveHotel");
                                }
                                else
                                {
                                    return RedirectToAction("HotelSearch", "Hotel");
                                }
                            }
                            else
                            {
                                return RedirectToAction(nameof(HomeController.Index), "Home");
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> OrgayholidayLogin(string usr_token) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Loginreq req = new Loginreq();
                    req.companyId = Convert.ToInt32(companyId);
                    req.password = usr_token;
                    req.userName = "sadsfsf";
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
                        HttpContext.Session.SetString("password", Convert.ToString(usr_token));
                        HttpContext.Session.SetString("OrderId", Convert.ToString(response.orderId));
                        HttpContext.Session.SetString("IDWiseDayAfter", Convert.ToString(response.IDWiseDayAfter));
                        if (response.isRedeem == true)
                        {
                            return RedirectToAction("Redemption", "Home");
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        ViewBag.message = "Please enter valid username and password";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = "Please enter valid username and password";
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
