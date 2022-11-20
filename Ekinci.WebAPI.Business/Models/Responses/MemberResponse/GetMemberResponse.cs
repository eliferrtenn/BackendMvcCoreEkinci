namespace Ekinci.WebAPI.Business.Models.Responses.MemberResponse
{
    public class GetMemberResponse
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ProfilePhotoUrl { get; set; }
    }
}