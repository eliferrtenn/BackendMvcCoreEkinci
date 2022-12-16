using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class SustainabilityController : CMSBaseController
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddSustainabilityRequest request, IFormFile PhotoUrl)
        {
            var result = await sustainabilityService.AddSustainability(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            var result = await sustainabilityService.GetSustainability();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSustainabilityRequest request,IFormFile PhotoUrl)
        {
            var result = await sustainabilityService.UpdateSustainability(request,PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
    }
}