using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.ProjectRequests
{
    public class AddProjectRequest
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ThumbUrl { get; set; }
        public string FileUrl { get; set; }
        public DateTime ProjectDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ApartmentCount { get; set; }
        public int SquareMeter { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public List<ProjectPhotosRequests> ProjectPhotos { get; set; }
    }
    public class ProjectPhotosRequests
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddProjectRequestValidator : AbstractValidator<AddProjectRequest>
    {
        public AddProjectRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.Title).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.Description).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.ThumbUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.ProjectDate).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.DeliveryDate).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.ApartmentCount).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
            RuleFor(x => x.SquareMeter).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}