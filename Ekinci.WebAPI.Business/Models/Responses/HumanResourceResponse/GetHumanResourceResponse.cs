namespace Ekinci.WebAPI.Business.Models.Responses.HumanResourceResponse
{
    public class GetHumanResourceResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}