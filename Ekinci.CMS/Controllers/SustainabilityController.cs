using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class SustainabilityController : Controller
    {
        private readonly ISustainabilityService sustainabilityService;

        public SustainabilityController(ISustainabilityService _sustainabilityService)
        {
            sustainabilityService = _sustainabilityService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await sustainabilityService.GetSustainability();
            return View(result.Data);
        }
        public async Task<IActionResult> Edit()
        {
            var result = await sustainabilityService.GetSustainability();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSustainabilityRequest request)
        {
            var result = await sustainabilityService.UpdateSustainability(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
    }
}