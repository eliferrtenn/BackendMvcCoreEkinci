namespace Ekinci.WebAPI.Business.Models.Requests.TechnicalServiceDemandRequests
{
    public class EditTechnicalServiceDemandRequest
    {
        public int ID { get; set; }
        public string DemandType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DemandUrgencyStatus { get; set; }
        public string SiteName { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentFloor { get; set; }
        public string ApartmentNo { get; set; }
        public string ContactInform { get; set; }
    }
}