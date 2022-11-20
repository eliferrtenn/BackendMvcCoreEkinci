namespace Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse
{
    public class GetIdentityGuideResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}