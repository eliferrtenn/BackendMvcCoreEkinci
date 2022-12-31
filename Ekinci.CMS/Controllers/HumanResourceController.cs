using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class HumanResourceController : CMSBaseController
    {
        private readonly IHumanResourceService humanResourceService;

        public HumanResourceController(IHumanResourceService _humanResourceService)
        {
            humanResourceService = _humanResourceService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await humanResourceService.GetHumanResource();
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddHumanResourceRequest request, IFormFile PhotoUrl)
        {
            var result = await humanResourceService.AddHumanResource(request, PhotoUrl);
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
            var result = await humanResourceService.UpdateHumanResource();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateHumanResourceRequest request, IFormFile PhotoUrl)
        {
            var result = await humanResourceService.UpdateHumanResource(request, PhotoUrl);
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