using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.KvkkRequests;
using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class KvkkController : CMSBaseController
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddKvkkRequest request, IFormFile PhotoUrl)
        {
            var result = await kvkkService.AddKvkk(request, PhotoUrl);
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
            var result = await kvkkService.GetKvkk();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKvkkRequest request, IFormFile PhotoUrl)
        {
            var result = await kvkkService.UpdateKvkk(request, PhotoUrl);
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