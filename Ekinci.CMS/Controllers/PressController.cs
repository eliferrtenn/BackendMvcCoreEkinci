using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class PressController : CMSBaseController
    {
        private readonly IPressService pressService;

        public PressController(IPressService _pressService)
        {
            pressService = _pressService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await pressService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await pressService.GetPress(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddPressRequest request,IFormFile PhotoUrl)
        {
            var result = await pressService.AddPress(request,PhotoUrl);
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
            var result = await pressService.GetPress(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePressRequest request, IFormFile PhotoUrl)
        {
            var result = await pressService.UpdatePress(request,PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeletePressRequest request)
        {
            var result = await pressService.DeletePress(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}