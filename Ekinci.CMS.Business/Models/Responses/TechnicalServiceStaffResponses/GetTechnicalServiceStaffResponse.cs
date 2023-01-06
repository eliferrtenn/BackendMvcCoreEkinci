namespace Ekinci.CMS.Business.Models.Responses.TechnicalServiceStaffResponses
{
    public class GetTechnicalServiceStaffResponse
    {
        public int ID { get; set; }
        public int TechnicalServiceNameID { get; set; }
        public string TechnicalServiceName { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
    }
}
