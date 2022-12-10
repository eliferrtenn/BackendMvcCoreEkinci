using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class PressController : Controller
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
        public async Task<IActionResult> Create(AddPressRequest request)
        {
            var result = await pressService.AddPress(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await pressService.GetPress(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePressRequest request)
        {
            var result = await pressService.UpdatePress(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
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