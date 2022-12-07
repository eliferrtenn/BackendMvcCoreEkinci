namespace Ekinci.CMS.Business.Models.Requests.HistoryRequests
{
    public class UpdateHistoryRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoUrl { get; set; }
    }
}