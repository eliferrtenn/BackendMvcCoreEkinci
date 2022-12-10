using BunnyCDN.Net.Storage;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryService historyService;

        public HistoryController(IHistoryService _historyService)
        {
            historyService = _historyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await historyService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await historyService.GetHistory(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddHistoryRequest request, IFormFile PhotoUrl)
        {          
            var result = await historyService.AddHistory(request,PhotoUrl);
            if (result.IsSuccess)
            {
                TempData["MessageIcon"] = "success";
                TempData["MessageText"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result.Message);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await historyService.GetHistory(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateHistoryRequest request, IFormFile PhotoUrl)
        {
            var result = await historyService.UpdateHistory(request,PhotoUrl);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteHistoryRequest request)
        {
            var result = await historyService.DeleteHistory(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}