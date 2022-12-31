using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.ProjectStatusRequests
{
    public class UpdateProjectStatusRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateProjectStatusRequestValidator : AbstractValidator<UpdateProjectStatusRequest>
    {
        public UpdateProjectStatusRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}