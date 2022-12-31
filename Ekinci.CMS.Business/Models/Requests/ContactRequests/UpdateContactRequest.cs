using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.ContactRequests
{
    public class UpdateContactRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MobilePhone { get; set; }
        public string LandPhone { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
    }
    public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
    {
        public UpdateContactRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Location).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}