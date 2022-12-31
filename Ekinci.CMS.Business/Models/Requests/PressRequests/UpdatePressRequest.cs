using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.PressRequests
{
    public class UpdatePressRequest
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdatePressRequestValidator : AbstractValidator<UpdatePressRequest>
    {
        public UpdatePressRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}