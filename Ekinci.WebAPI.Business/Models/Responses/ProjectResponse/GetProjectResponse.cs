namespace Ekinci.WebAPI.Business.Models.Responses.ProjectResponse
{
    public class GetProjectResponse
    {
        public int ID { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public DateTime? ProjectDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int ApartmentCount { get; set; }
        public int SquareMeter { get; set; }
    }
}