using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.VideosRequests
{
    public class AddVideosRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddVideosRequestValidator : AbstractValidator<AddVideosRequest>
    {
        public AddVideosRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.VideoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}