using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class IdentityGuideController : CMSBaseController
    {
        private readonly IIdentityGuideService identityGuideService;

        public IdentityGuideController(IIdentityGuideService _identityGuideService)
        {
            identityGuideService = _identityGuideService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await identityGuideService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await identityGuideService.GetIdentityGuide(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddIdentityGuideRequest request, IFormFile PhotoUrl)
        {
            var result = await identityGuideService.AddIdentityGuide(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await identityGuideService.GetIdentityGuide(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateIdentityGuideRequest request, IFormFile PhotoUrl)
        {
            var result = await identityGuideService.UpdateIdentityGuide(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteIdentityGuideRequest request)
        {
            var result = await identityGuideService.DeleteIdentityGuide(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }



    }
}