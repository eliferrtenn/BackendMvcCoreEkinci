using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceStaffRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class TechnicalServiceStaffController : CMSBaseController
    {
        private readonly ITechnicalServiceStaffService technicalServiceStaffService;
        private readonly ITechnicalServiceNameService technicalServiceNameService;

        public TechnicalServiceStaffController(ITechnicalServiceStaffService _technicalServiceStaffService, ITechnicalServiceNameService _technicalServiceNameService)
        {
            technicalServiceStaffService = _technicalServiceStaffService;
            technicalServiceNameService = _technicalServiceNameService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await technicalServiceStaffService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await technicalServiceStaffService.GetTechnicalServiceStaff(id);
            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            var result1 = await technicalServiceNameService.GetAll();
            ViewBag.TechnicalServiceStaffID = new SelectList(result1.Data, "ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTechnicalServiceStaffRequest request)
        {
            var result = await technicalServiceStaffService.AddTechnicalServiceStaff(request);
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
            var result1 = await technicalServiceNameService.GetAll();
            ViewBag.TechnicalServiceNameID = new SelectList(result1.Data, "ID", "Name");
            var result = await technicalServiceStaffService.UpdateTechnicalServiceStaff(id);
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTechnicalServiceStaffRequest request)
        {
            var result = await technicalServiceStaffService.UpdateTechnicalServiceStaff(request);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteTechnicalServiceStaffRequest request)
        {
            var result = await technicalServiceStaffService.DeleteTechnicalServiceStaff(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}
