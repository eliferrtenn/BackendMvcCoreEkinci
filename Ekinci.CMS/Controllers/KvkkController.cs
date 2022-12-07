using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class KvkkController : Controller
    {
        private readonly IKvkkService kvkkService;

        public KvkkController(IKvkkService _kvkkService)
        {
            kvkkService = _kvkkService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await kvkkService.GetKvkk();
            return View(result.Data);
        }
        public async Task<IActionResult> Edit()
        {
            var result = await kvkkService.GetKvkk();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKvkkResponse request)
        {
            var result = await kvkkService.UpdateKvkk(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
    }
}
