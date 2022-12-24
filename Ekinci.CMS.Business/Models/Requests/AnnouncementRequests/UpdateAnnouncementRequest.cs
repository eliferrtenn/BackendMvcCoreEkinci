namespace Ekinci.CMS.Business.Models.Requests.AnnouncementRequests
{
    public class UpdateAnnouncementRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbUrl { get; set; }
        public DateTime? AnnouncementDate { get; set; }
        public List<AnnouncementRequests> AnnouncementPhotos { get; set; }
    }
    public class AnnouncementRequests
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
}