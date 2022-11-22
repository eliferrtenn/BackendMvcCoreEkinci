using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonService commonService;

        public CommonController(ICommonService CommonService)
        {
            commonService = CommonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlog()
        {
            var result = await commonService.GetBlog();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetHumanResorce()
        {
            var result = await commonService.GetHumanResorce();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetIdentityGuide()
        {
            var result = await commonService.GetIdentityGuide();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetIntro()
        {
            var result = await commonService.GetIntro();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetKvkk()
        {
            var result = await commonService.GetKvkk();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSustainability()
        {
            var result = await commonService.GetSustainability();
            return View(result.Data);
        }
    }
}