using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class CommercialAreasController : Controller
    {
        private readonly ICommercialAreaService commercialAreaService;

        public CommercialAreasController(ICommercialAreaService CommercialAreaService)
        {
            commercialAreaService = CommercialAreaService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await commercialAreaService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommercialArea(int CommercialAreaID)
        {
            var result = await commercialAreaService.GetCommercialArea(CommercialAreaID);
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