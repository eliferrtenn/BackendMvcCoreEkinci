namespace Ekinci.CMS.Business.Models.Responses.UserResponses
{
    public class GetUserResponse
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
    }
}