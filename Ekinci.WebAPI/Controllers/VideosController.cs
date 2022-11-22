using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class VideosController : Controller
    {
        private readonly IVideosService videosService;

        public VideosController(IVideosService VideosService)
        {
            videosService = VideosService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await videosService.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetVideo(int videoID)
        {
            var result = await videosService.GetVideo(videoID);
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