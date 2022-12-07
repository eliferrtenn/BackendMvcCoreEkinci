namespace Ekinci.CMS.Business.Models.Requests.IntroRequests
{
    public class UpdateIntroRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
    }
}