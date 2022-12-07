using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class IntroController : Controller
    {
        private readonly IIntroService introService;

        public IntroController(IIntroService _introService)
        {
            introService = _introService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await introService.GetIntro();
            return View(result.Data);
        }
        public async Task<IActionResult> Edit()
        {
            var result = await introService.GetIntro();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateIntroRequest request)
        {
            var result = await introService.UpdateIntro(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
    }
}