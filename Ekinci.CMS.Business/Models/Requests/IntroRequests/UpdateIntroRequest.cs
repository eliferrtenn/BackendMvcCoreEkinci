using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.IntroRequests
{
    public class UpdateIntroRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public double SquareMeter { get; set; }
        public int YearCount { get; set; }
        public double CommercialAreaCount { get; set; }
    }
    public class UpdateIntroRequestValidator : AbstractValidator<UpdateIntroRequest>
    {
        public UpdateIntroRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Description).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.SquareMeter).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.YearCount).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}