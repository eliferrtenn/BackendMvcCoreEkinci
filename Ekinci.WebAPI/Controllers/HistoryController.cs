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
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetHistory(int historyID)
        {
            var result = await historyService.GetHistory(historyID);
            return Ok(result);
        }
    }
}