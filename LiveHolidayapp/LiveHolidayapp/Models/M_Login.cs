using System.ComponentModel.DataAnnotations;

namespace LiveHolidayapp.Models
{
    public class M_Login
    {
        [Required(ErrorMessage = "Please Enter Username..")]
        [DataType(DataType.Text)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class Loginreq
    {
        public string? userName { get; set; }
        public string? password { get; set; }
        public int companyId { get; set; }
    }

    public class Loginresponse
    {

        public int formNo { get; set; }
        public string name { get; set; }
        public bool isRedeem { get; set; }
        public int kitId { get; set; }
        public bool isCompany { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public DateTime doj { get; set; }
        public DateTime expireDate { get; set; }
        public int id { get; set; }
        public int orderId { get; set; }
        public string tokenString { get; set; }
        public int IDWiseDayAfter { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }

    }
    public class Rootobject
    {
        public int code { get; set; }
        public Data data { get; set; }
        public object message { get; set; }
        public object error { get; set; }
    }

    public class Data
    {
        public int formNo { get; set; }
        public string name { get; set; }
        public bool isRedeem { get; set; }
        public int kitId { get; set; }
        public bool isCompany { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public DateTime doj { get; set; }
        public DateTime expireDate { get; set; }
        public int id { get; set; }
        public int orderId { get; set; }
        public string tokenString { get; set; }
    }
}
