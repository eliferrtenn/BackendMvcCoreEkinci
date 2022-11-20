namespace Ekinci.WebAPI.Business.Models.Responses.SustainabilityResponse
{
    public class GetSustainabilityResponse
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}