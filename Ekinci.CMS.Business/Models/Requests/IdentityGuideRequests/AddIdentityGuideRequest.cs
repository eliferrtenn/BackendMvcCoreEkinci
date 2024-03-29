﻿using Ekinci.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests
{
    public class AddIdentityGuideRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class AddIdentityGuideRequestValidator : AbstractValidator<AddIdentityGuideRequest>
    {
        public AddIdentityGuideRequestValidator(IStringLocalizer<CommonResource> _localizer)
        {
            RuleFor(x => x.PhotoUrl).NotNull().WithMessage(x => _localizer["ValidationForRequired"]);
        }
    }
}
