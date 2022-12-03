namespace Ekinci.CMS.Business.Models.Requests.BlogRequests
{
    public class UpdateBlogRequest
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public DateTime? BlogDate { get; set; }
        public string? InstagramUrl { get; set; }
        public string? PhotoUrl { get; set; }
    }
}