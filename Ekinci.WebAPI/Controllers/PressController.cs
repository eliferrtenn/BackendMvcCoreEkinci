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
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPress(int pressID)
        {
            var result = await pressService.GetPress(pressID);
            return Ok(result);
        }
    }
}