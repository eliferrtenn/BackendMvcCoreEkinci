namespace Ekinci.WebAPI.Business.Models.Responses.IntroResponse
{
    public class GetIntroResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public int SquareMeter { get; set; }
        public int YearCount { get; set; }
        public int CommercialAreaCount { get; set; }
    }
}