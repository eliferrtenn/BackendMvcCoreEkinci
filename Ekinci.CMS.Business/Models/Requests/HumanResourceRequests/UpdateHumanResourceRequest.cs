using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.HumanResourceRequests
{
    public class UpdateHumanResourceRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateHumanResourceRequestValidator : AbstractValidator<UpdateHumanResourceRequest>
    {
        public UpdateHumanResourceRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Description).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}