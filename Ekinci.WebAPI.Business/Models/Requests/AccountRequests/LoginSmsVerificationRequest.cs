namespace Ekinci.WebAPI.Business.Models.Requests.AccountRequests
{
    public class LoginSmsVerificationRequest
    {
        public string MobilePhone { get; set; }
        public string SmsCode { get; set; }
    }
}