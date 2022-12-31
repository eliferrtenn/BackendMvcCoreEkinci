using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests
{
    public class UpdateIdentityGuideRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateIdentityGuideRequestValidator : AbstractValidator<UpdateIdentityGuideRequest>
    {
        public UpdateIdentityGuideRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}