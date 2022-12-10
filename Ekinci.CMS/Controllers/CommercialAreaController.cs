using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class CommercialAreaController : Controller
    {
        private readonly ICommercialAreaService commercialAreaService;

        public CommercialAreaController(ICommercialAreaService _commercialAreaService)
        {
            commercialAreaService = _commercialAreaService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await commercialAreaService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await commercialAreaService.GetCommercialArea(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddCommercialAreaRequest request)
        {
            var result = await commercialAreaService.AddCommercialArea(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await commercialAreaService.GetCommercialArea(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCommercialAreaRequest request)
        {
            var result = await commercialAreaService.UpdateCommercialArea(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteCommercialAreaRequest request)
        {
            var result = await commercialAreaService.DeleteCommercialArea(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}