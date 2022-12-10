namespace Ekinci.CMS.Business.Models.Requests.AnnouncementRequests
{
    public class AddAnnouncementRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<AnnouncemenPhototRequest> AnnouncementPhotos { get; set; }
    }
    public class AnnouncemenPhototRequest
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
}
