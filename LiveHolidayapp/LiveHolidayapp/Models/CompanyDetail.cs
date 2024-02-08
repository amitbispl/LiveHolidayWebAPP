using LiveHolidayapp.DataAccess;
using LiveHolidayapp.Repository;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.Design;
using System.Data;

namespace LiveHolidayapp.Models
{
    public class CompanyDetail
    {
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private I_adminHandler _AdminHandler;
        public CompanyDetail(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _AdminHandler = new R_adminHandler();
        }


        public M_Company GetCompany()
        {
            M_Company obj = new M_Company();
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Path.GetDirectoryName(typeof(BLLDBOperations).Assembly.Location))
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration config = configuration.Build();
            string? compid = "0";
            string? URL = "";
            URL = _httpContextAccessor.HttpContext?.Request.Host.Value.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("/", "").Replace("Utility.", "").Replace("Care.", "");
            DataSet ds;
            if (URL == "LOCALHOST:7035")
            {
                compid = config.GetValue<string>("CompanyId");
                ds = _AdminHandler.GetCompanySetting(compid, "");
            }
            else
            {
                ds = _AdminHandler.GetCompanySetting("0", URL);
            }
            obj.Title = Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]);
            obj.CompanyName = Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]);
            obj.Address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]);
            obj.CMEmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]);
            obj.CMMobileNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
            obj.AppDomain = Convert.ToString(ds.Tables[0].Rows[0]["AppDomain"]);
            obj.Logo = Convert.ToString(ds.Tables[0].Rows[0]["Logo"]);
            obj.CompanyId = Convert.ToString(ds.Tables[0].Rows[0]["CompanyId"]);
            obj.APIURL = Convert.ToString(ds.Tables[0].Rows[0]["ApIUrl"]);
            obj.Token = Convert.ToString(ds.Tables[0].Rows[0]["Token"]);
            obj.FBLink = Convert.ToString(ds.Tables[0].Rows[0]["FBLink"]);
            obj.WALink = Convert.ToString(ds.Tables[0].Rows[0]["WALink"]);
            obj.TWLink = Convert.ToString(ds.Tables[0].Rows[0]["TWLink"]);
            obj.INLink = Convert.ToString(ds.Tables[0].Rows[0]["INLink"]);
            obj.RedirectDomain = Convert.ToString(ds.Tables[0].Rows[0]["RedirectDomain"]);
            obj.ISchild = Convert.ToString(ds.Tables[0].Rows[0]["ISChild"]);
            obj.DayAfter = Convert.ToInt32(ds.Tables[0].Rows[0]["DayAfter"]);
            obj.PriceRangeStart = Convert.ToString(ds.Tables[0].Rows[0]["PriceRangeStart"]);
            obj.PriceRangeEnd = Convert.ToString(ds.Tables[0].Rows[0]["PriceRangeEnd"]);
            //_httpContextAccessor.HttpContext.Session.SetString("CompanySetting") .Session["CompanySetting"] = ds;

            _httpContextAccessor.HttpContext.Session.SetString("Title", Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]));
            _httpContextAccessor.HttpContext.Session.SetString("CompanyName",Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]));
            _httpContextAccessor.HttpContext.Session.SetString("Address",Convert.ToString(ds.Tables[0].Rows[0]["Address"]));
            _httpContextAccessor.HttpContext.Session.SetString("CMEmailID",Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]));
            _httpContextAccessor.HttpContext.Session.SetString("CMMobileNo",Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]));
            _httpContextAccessor.HttpContext.Session.SetString("logo",Convert.ToString(ds.Tables[0].Rows[0]["logo"]));
            _httpContextAccessor.HttpContext.Session.SetString("CompanyId",Convert.ToString(ds.Tables[0].Rows[0]["CompanyId"]));
            _httpContextAccessor.HttpContext.Session.SetString("City",Convert.ToString(ds.Tables[0].Rows[0]["city"]));
            _httpContextAccessor.HttpContext.Session.SetString("StateName",Convert.ToString(ds.Tables[0].Rows[0]["StateName"]));
            _httpContextAccessor.HttpContext.Session.SetString("PinCode",Convert.ToString(ds.Tables[0].Rows[0]["PinCode"]));
            _httpContextAccessor.HttpContext.Session.SetString("FBLink",Convert.ToString(ds.Tables[0].Rows[0]["FBLink"]));
            _httpContextAccessor.HttpContext.Session.SetString("WALink",Convert.ToString(ds.Tables[0].Rows[0]["WALink"]));
            _httpContextAccessor.HttpContext.Session.SetString("TWLink", Convert.ToString(ds.Tables[0].Rows[0]["TWLink"]));
            _httpContextAccessor.HttpContext.Session.SetString("INLink",Convert.ToString(ds.Tables[0].Rows[0]["INLink"]));
            _httpContextAccessor.HttpContext.Session.SetString("Landline",Convert.ToString(ds.Tables[0].Rows[0]["Landline"]));
            _httpContextAccessor.HttpContext.Session.SetString("RedirectDomain",Convert.ToString(ds.Tables[0].Rows[0]["RedirectDomain"]));
            _httpContextAccessor.HttpContext.Session.SetString("IsAdditionalService",Convert.ToString(ds.Tables[0].Rows[0]["IsAdditionalService"]));
            _httpContextAccessor.HttpContext.Session.SetString("IsInternational",Convert.ToString(ds.Tables[0].Rows[0]["IsInternational"]));
            _httpContextAccessor.HttpContext.Session.SetString("redirectUrl",Convert.ToString(ds.Tables[0].Rows[0]["Rediectpage"]));
            _httpContextAccessor.HttpContext.Session.SetString("IsCustomizeDuration",Convert.ToString(ds.Tables[0].Rows[0]["IsCustomizeDuration"]));
            _httpContextAccessor.HttpContext.Session.SetString("ISchild", Convert.ToString(ds.Tables[0].Rows[0]["ISchild"]));
            _httpContextAccessor.HttpContext.Session.SetString("DayAfter", Convert.ToString(ds.Tables[0].Rows[0]["DayAfter"]));
            _httpContextAccessor.HttpContext.Session.SetString("PriceRangeStart", Convert.ToString(ds.Tables[0].Rows[0]["PriceRangeStart"]));
            _httpContextAccessor.HttpContext.Session.SetString("PriceRangeEnd", Convert.ToString(ds.Tables[0].Rows[0]["PriceRangeEnd"]));
            _httpContextAccessor.HttpContext.Session.SetString("IsOffLineBook", Convert.ToString(ds.Tables[0].Rows[0]["IsOffLineBook"]));
            _httpContextAccessor.HttpContext.Session.SetString("StartAfterday", Convert.ToString(ds.Tables[0].Rows[0]["StartAfterday"]));
            _httpContextAccessor.HttpContext.Session.SetString("GMapURL", Convert.ToString(ds.Tables[0].Rows[0]["GMapURL"]));
            _httpContextAccessor.HttpContext.Session.SetString("GMapIframeURL", Convert.ToString(ds.Tables[0].Rows[0]["GMapIframeURL"]));
            return obj;
        }
    }
}
