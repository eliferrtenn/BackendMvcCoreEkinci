using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.BlogRequests
{
    public class AddBlogRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime BlogDate { get; set; }
        public string InstagramUrl { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddBlogRequestValidator : AbstractValidator<AddBlogRequest>
    {
        public AddBlogRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.InstagramUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}