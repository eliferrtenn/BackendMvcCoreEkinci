namespace Ekinci.CMS.Business.Models.Requests.HumanResourceRequests
{
    public class AddHumanResourceRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
    }
}