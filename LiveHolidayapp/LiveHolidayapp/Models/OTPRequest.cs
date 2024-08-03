namespace LiveHolidayapp.Models
{
    public class OTPRequest
    {
        public string FormNo { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public int CompanyId { get; set; }
    }

    public class OTPREsponse
    {
        public int ID { get; set; }
    }
    public class ValidOTPReq
    {
        public string OTP { get; set; }
        public int ID { get; set; }
    }
}
