using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class CommercialAreaController : CMSBaseController
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
        public async Task<IActionResult> Create(AddCommercialAreaRequest request,IFormFile PhotoUrl)
        {
            var result = await commercialAreaService.AddCommercialArea(request, PhotoUrl);
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
            var result = await commercialAreaService.GetCommercialArea(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCommercialAreaRequest request,IFormFile PhotoUrl)
        {
            var result = await commercialAreaService.UpdateCommercialArea(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
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