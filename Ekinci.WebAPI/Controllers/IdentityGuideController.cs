using System;
using Microsoft.AspNetCore.Mvc;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.Common.BaseController;

namespace Ekinci.WebAPI.Controllers
{
	public class IdentityGuideController : APIBaseController
    {
        private readonly IIdentityGuideService identityGuideService;

        public IdentityGuideController(IIdentityGuideService _identityGuideService)
        {
            identityGuideService = _identityGuideService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await identityGuideService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommercialArea(int identityGuideID)
        {
            var result = await identityGuideService.GetIdentityGuide(identityGuideID);
            return Ok(result);
        }

    }
}

