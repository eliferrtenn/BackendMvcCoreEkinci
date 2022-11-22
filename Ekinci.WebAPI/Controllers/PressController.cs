using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class PressController : Controller
    {
        private readonly IPressService pressService;

        public PressController(IPressService PressService)
        {
            pressService = PressService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await pressService.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPress(int pressID)
        {
            var result = await pressService.GetPress(pressID);
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