namespace Ekinci.CMS.Business.Models.Requests.IntroRequests
{
    public class AddIntroRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public float SquareMeter { get; set; }
        public int YearCount { get; set; }
        public float CommercialAreaCount { get; set; }
    }
}
