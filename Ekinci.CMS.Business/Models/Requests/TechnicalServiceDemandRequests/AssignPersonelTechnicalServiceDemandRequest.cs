using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests
{
    public class AssignPersonelTechnicalServiceDemandRequest
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string DemandType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DemandUrgencyStatus { get; set; }
        public string SiteName { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentFloor { get; set; }
        public string ApartmentNo { get; set; }
        public string ContactInform { get; set; }
        public DateTime CreateDayDemand { get; set; }
        public DateTime? SolutionDayDemand { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
    }
    public class AssignPersonelTechnicalServiceDemandRequestValidator : AbstractValidator<AssignPersonelTechnicalServiceDemandRequest>
    {
        public AssignPersonelTechnicalServiceDemandRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.FullName).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.MobilePhone).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}