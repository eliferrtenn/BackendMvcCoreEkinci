using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class IntroController : CMSBaseController
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


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddIntroRequest request, IFormFile PhotoUrl)
        {
            var result = await introService.AddIntro(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            var result = await introService.GetIntro();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateIntroRequest request,IFormFile PhotoUrl)
        {
            var result = await introService.UpdateIntro(request,PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
    }
}