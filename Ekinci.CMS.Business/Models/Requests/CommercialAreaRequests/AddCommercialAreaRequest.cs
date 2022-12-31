using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests
{
    public class AddCommercialAreaRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddCommercialAreaRequestValidator : AbstractValidator<AddCommercialAreaRequest>
    {
        public AddCommercialAreaRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}