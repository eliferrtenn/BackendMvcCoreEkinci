using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class HistoryController : CMSBaseController
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
            var result = await historyService.AddHistory(request, PhotoUrl);
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
            var result = await historyService.GetHistory(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateHistoryRequest request, IFormFile PhotoUrl)
        {
            var result = await historyService.UpdateHistory(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
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