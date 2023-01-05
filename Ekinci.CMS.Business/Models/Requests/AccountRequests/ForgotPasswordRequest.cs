using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.AccountRequests
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Email).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}