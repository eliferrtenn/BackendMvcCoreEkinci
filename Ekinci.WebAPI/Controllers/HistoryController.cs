using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class HistoryController : Controller
    {

        private readonly IHistoryService historyService;

        public HistoryController(IHistoryService HistoryService)
        {
            historyService = HistoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await historyService.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetHistory(int historyID)
        {
            var result = await historyService.GetHistory(historyID);
            if (result.IsSuccess)
            {
                TempData["MessageText"] = result.Message;
                return View(result.Data);
            }
            TempData["MessageText"] = result.Message;
            return View();
        }
    }
}