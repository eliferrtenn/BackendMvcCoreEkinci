namespace Ekinci.WebAPI.Business.Models.Responses.PressResponse
{
    public class GetPressResponse
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}