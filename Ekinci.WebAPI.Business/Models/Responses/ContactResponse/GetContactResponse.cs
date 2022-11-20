namespace Ekinci.WebAPI.Business.Models.Responses.ContactResponse
{
    public class GetContactResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MobilePhone { get; set; }
        public string LandPhone { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}