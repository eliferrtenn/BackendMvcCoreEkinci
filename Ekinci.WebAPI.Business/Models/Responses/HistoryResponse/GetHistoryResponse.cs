namespace Ekinci.WebAPI.Business.Models.Responses.HistoryResponse
{
    public class GetHistoryResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}