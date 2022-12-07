namespace Ekinci.CMS.Business.Models.Responses.ContactResponses
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
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
    }
}