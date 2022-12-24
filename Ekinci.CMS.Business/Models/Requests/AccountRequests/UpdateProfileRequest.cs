using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Models.Requests.AccountRequests
{
    public class UpdateProfileRequest
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string ProfilePhotoUrl { get; set; }
    }
}