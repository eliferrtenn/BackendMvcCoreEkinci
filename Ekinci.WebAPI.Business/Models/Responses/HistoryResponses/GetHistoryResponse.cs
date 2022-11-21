namespace Ekinci.WebAPI.Business.Models.Responses.HistoryResponse
{
    public class GetHistoryResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PhotoUrl { get; set; }
    }
}