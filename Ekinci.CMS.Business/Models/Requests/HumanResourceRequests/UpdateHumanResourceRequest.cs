namespace Ekinci.CMS.Business.Models.Requests.HumanResourceRequests
{
    public class UpdateHumanResourceRequest
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}