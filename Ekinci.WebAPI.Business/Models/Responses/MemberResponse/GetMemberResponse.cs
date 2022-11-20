namespace Ekinci.WebAPI.Business.Models.Responses.MemberResponse
{
    public class GetMemberResponse
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string MobileDevicePushID { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}