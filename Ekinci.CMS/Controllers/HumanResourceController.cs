using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class HumanResourceController : Controller
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
        public async Task<IActionResult> Edit()
        {
            var result = await humanResourceService.GetHumanResource();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateHumanResourceRequest request)
        {
            var result = await humanResourceService.UpdateHumanResource(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
    }
}