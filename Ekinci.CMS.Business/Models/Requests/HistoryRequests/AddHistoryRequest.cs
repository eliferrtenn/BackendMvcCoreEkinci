using Ekinci.CMS.Business.Models.Requests.ContactRequests;
using Ekinci.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.HistoryRequests
{
    public class AddHistoryRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddHistoryRequestValidator : AbstractValidator<AddHistoryRequest>
    {
        public AddHistoryRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.StartDate).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}