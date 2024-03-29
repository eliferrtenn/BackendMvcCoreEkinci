﻿namespace Ekinci.WebAPI.Business.Models.Requests.TechnicalServiceDemandRequests
{
    public class AddTechnicalServiceDemandRequest
    {
        public string DemandType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DemandUrgencyStatus { get; set; }
        public string SiteName { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentFloor { get; set; }
        public string ApartmentNo { get; set; }
        public string ContactInform { get; set; }
        public DateTime CreateDayDemand { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}