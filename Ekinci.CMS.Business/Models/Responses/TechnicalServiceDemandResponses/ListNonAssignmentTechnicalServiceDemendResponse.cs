namespace Ekinci.CMS.Business.Models.Responses.TechnicalServiceDemandResponses
{
    public class ListNonAssignmentTechnicalServiceDemendResponse
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int TechnicalServiceStaffID { get; set; }
        public string TechnicalServiceName { get; set; }
        public string DemandType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DemandUrgencyStatus { get; set; }
        public string SiteName { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentFloor { get; set; }
        public string ApartmentNo { get; set; }
        public string ContactInform { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
        public DateTime CreateDayDemand { get; set; }
        public DateTime? SolutionDayDemand { get; set; }
    }
}