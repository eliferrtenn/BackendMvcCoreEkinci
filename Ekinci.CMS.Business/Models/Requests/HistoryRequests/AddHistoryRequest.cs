using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Models.Requests.HistoryRequests
{
    public class AddHistoryRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoUrl { get; set; }
    }
}