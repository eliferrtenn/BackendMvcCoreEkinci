using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.AnnouncementRequests
{
    public class AddAnnouncementRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbUrl { get; set; }
        public DateTime? AnnouncementDate { get; set; }
        public List<AnnouncemenPhototRequest> AnnouncementPhotos { get; set; }
    }
    public class AnnouncemenPhototRequest
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddAnnouncementRequestValidator : AbstractValidator<AddAnnouncementRequest>
    {
        public AddAnnouncementRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.ThumbUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Description).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}
