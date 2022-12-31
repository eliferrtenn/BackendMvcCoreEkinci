using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests
{
    public class UpdateCommercialAreaRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateCommercialAreaRequestValidator : AbstractValidator<UpdateCommercialAreaRequest>
    {
        public UpdateCommercialAreaRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}