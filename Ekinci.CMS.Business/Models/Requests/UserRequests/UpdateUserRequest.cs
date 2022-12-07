namespace Ekinci.CMS.Business.Models.Requests.UserRequests
{
    public class UpdateUserRequest
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? MobilePhone { get; set; }
        public string? Password { get; set; }
    }
}