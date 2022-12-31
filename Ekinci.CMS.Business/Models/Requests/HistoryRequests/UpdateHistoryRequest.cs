using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.HistoryRequests
{
    public class UpdateHistoryRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateHistoryRequestValidator : AbstractValidator<UpdateHistoryRequest>
    {
        public UpdateHistoryRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.StartDate).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}