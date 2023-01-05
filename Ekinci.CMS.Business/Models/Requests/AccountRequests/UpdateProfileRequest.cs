using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.AccountRequests
{
    public class UpdateProfileRequest
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string ProfilePhotoUrl { get; set; }
    }
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Firstname).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Lastname).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Email).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.MobilePhone).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}