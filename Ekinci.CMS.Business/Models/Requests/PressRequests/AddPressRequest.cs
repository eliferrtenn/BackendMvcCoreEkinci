using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.PressRequests
{
    public class AddPressRequest
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddPressRequestValidator : AbstractValidator<AddPressRequest>
    {
        public AddPressRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}