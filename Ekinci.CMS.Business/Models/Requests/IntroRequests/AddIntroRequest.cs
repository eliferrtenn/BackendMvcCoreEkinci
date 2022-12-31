using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.IntroRequests
{
    public class AddIntroRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public float SquareMeter { get; set; }
        public int YearCount { get; set; }
        public float CommercialAreaCount { get; set; }
    }
    public class AddIntroRequestValidator : AbstractValidator<AddIntroRequest>
    {
        public AddIntroRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Description).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.SquareMeter).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.YearCount).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}