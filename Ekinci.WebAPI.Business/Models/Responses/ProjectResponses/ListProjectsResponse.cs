namespace Ekinci.WebAPI.Business.Models.Responses.ProjectResponse
{
    public class ListProjectsResponse
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ProjectDate { get; set; }
        public string DeliveryDate { get; set; }
        public int ApartmentCount { get; set; }
        public int SquareMeter { get; set; }
    }
}